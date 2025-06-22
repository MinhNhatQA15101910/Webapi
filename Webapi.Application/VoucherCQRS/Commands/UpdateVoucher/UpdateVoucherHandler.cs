using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Entities;
using Webapi.Domain.Factories;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs.Voucher;

namespace Webapi.Application.VoucherCQRS.Commands.UpdateVoucher;

public class UpdateVoucherHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    VoucherFactory voucherFactory // Add the VoucherFactory
) : ICommandHandler<UpdateVoucherCommand, VoucherDto>
{
    public async Task<VoucherDto> Handle(UpdateVoucherCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get existing voucher with its related type and items
            var voucher = await unitOfWork.VoucherRepository.GetVoucherWithDetailsAsync(request.VoucherId, cancellationToken)
                ?? throw new BadRequestException($"Voucher with ID {request.VoucherId} not found");

            // Use the flyweight pattern to get or create the updated voucher type
            var expireDate = request.VoucherDto.ExpiredAt ?? voucher.ExpiredAt;
            var voucherType = voucherFactory.GetVoucherType(
                request.VoucherDto.Name,
                request.VoucherDto.Value,
                expireDate
            );
            
            // Update the voucher properties
            voucher.TypeId = voucherType.Id;
            voucher.Type = voucherType;
            voucher.Quantity = request.VoucherDto.Quantity;
            voucher.ExpiredAt = expireDate;
            voucher.UpdatedAt = DateTime.UtcNow;
            
            // If the voucher type changed or quantity changed, we may need to regenerate the voucher items
            if (voucher.TypeId != voucherType.Id || voucher.Items.Count != request.VoucherDto.Quantity)
            {
                // Get existing items
                var existingItems = await unitOfWork.VoucherItemRepository.GetByVoucherIdAsync(voucher.Id, cancellationToken);
                
                // Remove existing items
                if (existingItems.Any())
                {
                    unitOfWork.VoucherItemRepository.RemoveRange(existingItems);
                    voucher.Items.Clear();
                }
                
                // Create a prototype item 
                var prototypeItem = new VoucherItem
                {
                    Id = Guid.NewGuid(),
                    VoucherId = voucher.Id,
                    Status = true,
                };
                
                // Generate new items using the prototype pattern
                var newItems = voucherFactory.GenerateVoucherItems(prototypeItem, request.VoucherDto.Quantity);
                
                // Add new items to repository
                unitOfWork.VoucherItemRepository.AddRange(newItems);
                
                // Update voucher's item collection
                voucher.Items = newItems.ToList();
            }
            
            // Update in repository
            unitOfWork.VoucherRepository.Update(voucher);
            
            // Save changes
            await unitOfWork.CompleteAsync(cancellationToken);
            
            // Return mapped DTO
            return mapper.Map<VoucherDto>(voucher);
        }
        catch (BadRequestException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BadRequestException($"Failed to update voucher: {ex.Message}");
        }
    }
}
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Factories;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs.Voucher;

namespace Webapi.Application.VoucherCQRS.Commands.CreateVoucher;

public class CreateVoucherHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    VoucherFactory voucherFactory
) : ICommandHandler<CreateVoucherCommand, VoucherDto>
{
    public async Task<VoucherDto> Handle(CreateVoucherCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate quantity
            if (request.VoucherDto.Quantity <= 0)
            {
                throw new BadRequestException("Voucher quantity must be greater than 0");
            }
            
            // Use the flyweight pattern to get or create the voucher type
            var expireDate = request.VoucherDto.ExpiredAt ?? DateTime.UtcNow.AddMonths(3);
            var voucherType = voucherFactory.GetVoucherType(
                request.VoucherDto.Name,
                request.VoucherDto.Value,
                expireDate
            );
            
            // Use the prototype pattern to create voucher with items
            var voucher = voucherFactory.CreateVoucher(
                voucherType, 
                request.VoucherDto.Quantity, 
                expireDate
            );
            
            // Explicitly set quantity to ensure it's not lost
            voucher.Quantity = request.VoucherDto.Quantity;
                
            // Add to repository
            unitOfWork.VoucherRepository.Add(voucher);
            
            // Save changes
            await unitOfWork.CompleteAsync(cancellationToken);
            
            // Get the freshly created voucher to ensure we have all data
            var createdVoucher = await unitOfWork.VoucherRepository.GetByIdAsync(voucher.Id, cancellationToken)
                ?? throw new BadRequestException("Failed to retrieve created voucher");
                
            // Return mapped DTO
            return mapper.Map<VoucherDto>(createdVoucher);
        }
        catch (BadRequestException)
        {
            // Rethrow these specific exceptions to maintain their status code
            throw;
        }
        catch (Exception ex)
        {
            // Catch any unexpected exceptions
            throw new BadRequestException($"Failed to create voucher: {ex.Message}");
        }
    }
}
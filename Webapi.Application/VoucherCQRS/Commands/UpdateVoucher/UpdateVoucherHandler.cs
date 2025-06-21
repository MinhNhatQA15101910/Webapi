using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs.Voucher;

namespace Webapi.Application.VoucherCQRS.Commands.UpdateVoucher;

public class UpdateVoucherHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<UpdateVoucherCommand, VoucherDto>
{
    public async Task<VoucherDto> Handle(UpdateVoucherCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get existing voucher
            var voucher = await unitOfWork.VoucherRepository.GetByIdAsync(request.VoucherId, cancellationToken)
                ?? throw new BadRequestException($"Voucher with ID {request.VoucherId} not found");

            // Update properties
            voucher.Name = request.VoucherDto.Name;
            voucher.Value = request.VoucherDto.Value;
            voucher.Quantity = request.VoucherDto.Quantity;
            
            if (request.VoucherDto.ExpiredAt.HasValue)
            {
                voucher.ExpiredAt = request.VoucherDto.ExpiredAt.Value;
            }
            
            voucher.UpdatedAt = DateTime.UtcNow;
            
            // Update in repository
            unitOfWork.VoucherRepository.Update(voucher);
            
            // Save changes
            await unitOfWork.CompleteAsync(cancellationToken);
            
            // Return mapped DTO
            return mapper.Map<VoucherDto>(voucher);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BadRequestException($"Failed to update voucher: {ex.Message}");
        }
    }
}
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs.Voucher;

namespace Webapi.Application.VoucherCQRS.Commands.CreateVoucher;

public class CreateVoucherHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<CreateVoucherCommand, VoucherDto>
{
    public async Task<VoucherDto> Handle(CreateVoucherCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Create new voucher
            var voucher = new Voucher
            {
                Id = Guid.NewGuid(),
                Name = request.VoucherDto.Name,
                Value = request.VoucherDto.Value,
                Quantity = request.VoucherDto.Quantity,
                ExpiredAt = request.VoucherDto.ExpiredAt ?? DateTime.UtcNow.AddMonths(3),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
                
            // Add to repository
            unitOfWork.VoucherRepository.Add(voucher);
            
            // Save changes
            await unitOfWork.CompleteAsync(cancellationToken);
                
            // Return mapped DTO
            return mapper.Map<VoucherDto>(voucher);
        }
        catch (Exception ex)
        {
            // Catch any unexpected exceptions
            throw new BadRequestException($"Failed to create voucher: {ex.Message}");
        }
    }
}
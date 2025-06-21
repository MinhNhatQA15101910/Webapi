using AutoMapper;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs.Voucher;

namespace Webapi.Application.VoucherCQRS.Queries.GetVoucherById;

public class GetVoucherByIdHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IQueryHandler<GetVoucherByIdQuery, VoucherDto>
{
    public async Task<VoucherDto> Handle(GetVoucherByIdQuery request, CancellationToken cancellationToken)
    {
        var voucher = await unitOfWork.VoucherRepository.GetByIdAsync(request.VoucherId, cancellationToken)
            ?? throw new BadRequestException($"Voucher with ID {request.VoucherId} not found");
            
        return mapper.Map<VoucherDto>(voucher);
    }
}
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
        var voucher = await unitOfWork.VoucherRepository.GetVoucherWithDetailsAsync(request.VoucherId, cancellationToken)
            ?? throw new BadRequestException($"Voucher with ID {request.VoucherId} not found");
        
        var voucherDto = mapper.Map<VoucherDto>(voucher);
        
        // Calculate available quantity based on unused items
        // Get voucherItems with same VoucherId - make sure to await the async call
        var items = await unitOfWork.VoucherItemRepository.GetByVoucherIdAsync(voucher.Id, cancellationToken);
        voucherDto.AvailableQuantity = items.Count(i => i.Status);
        
        return voucherDto;
    }
}
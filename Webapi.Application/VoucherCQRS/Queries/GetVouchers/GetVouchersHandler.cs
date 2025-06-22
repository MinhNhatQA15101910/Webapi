using AutoMapper;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs.Voucher;

namespace Webapi.Application.VoucherCQRS.Queries.GetVouchers;

public class GetVouchersHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IQueryHandler<GetVouchersQuery, IEnumerable<VoucherDto>>
{
    public async Task<IEnumerable<VoucherDto>> Handle(GetVouchersQuery request, CancellationToken cancellationToken)
    {
        // Get all vouchers without items to avoid excessive data loading
        var vouchers = await unitOfWork.VoucherRepository.GetAllAsync(cancellationToken);
        var voucherDtos = mapper.Map<IEnumerable<VoucherDto>>(vouchers).ToList();
        
        // Calculate available quantities for each voucher
        foreach (var voucherDto in voucherDtos)
        {
            // Get and count available items for each voucher
            var items = await unitOfWork.VoucherItemRepository.GetByVoucherIdAsync(voucherDto.Id, cancellationToken);
            voucherDto.AvailableQuantity = items.Count(i => i.Status);
        }
        
        return voucherDtos;
    }
}
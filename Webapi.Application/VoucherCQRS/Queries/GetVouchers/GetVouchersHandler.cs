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
        var vouchers = await unitOfWork.VoucherRepository.GetAllAsync(cancellationToken);
        return mapper.Map<IEnumerable<VoucherDto>>(vouchers);
    }
}
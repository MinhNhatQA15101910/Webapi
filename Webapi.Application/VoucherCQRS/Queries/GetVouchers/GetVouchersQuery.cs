using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs.Voucher;

namespace Webapi.Application.VoucherCQRS.Queries.GetVouchers;

public record GetVouchersQuery : IQuery<IEnumerable<VoucherDto>>;
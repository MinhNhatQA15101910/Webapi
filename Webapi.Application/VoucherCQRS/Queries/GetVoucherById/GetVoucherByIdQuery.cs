using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs.Voucher;

namespace Webapi.Application.VoucherCQRS.Queries.GetVoucherById;

public record GetVoucherByIdQuery(Guid VoucherId) : IQuery<VoucherDto>;
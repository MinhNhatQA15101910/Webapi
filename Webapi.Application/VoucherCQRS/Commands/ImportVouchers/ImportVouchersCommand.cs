using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs.Voucher;

namespace Webapi.Application.VoucherCQRS.Commands.ImportVouchers;

public record ImportVouchersCommand(ImportVoucherDto ImportDto) : ICommand<int>;
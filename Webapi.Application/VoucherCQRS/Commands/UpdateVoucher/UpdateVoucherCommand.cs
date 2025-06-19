using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs.Voucher;

namespace Webapi.Application.VoucherCQRS.Commands.UpdateVoucher;

public record UpdateVoucherCommand(Guid VoucherId, UpdateVoucherDto VoucherDto) : ICommand<VoucherDto>;
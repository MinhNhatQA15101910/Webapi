using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs.Voucher;

namespace Webapi.Application.VoucherCQRS.Commands.CreateVoucher;

public record CreateVoucherCommand(CreateVoucherDto VoucherDto) : ICommand<VoucherDto>;
using Webapi.Application.Common.Interfaces.MediatR;

namespace Webapi.Application.VoucherCQRS.Commands.DeleteVoucher;

public record DeleteVoucherCommand(Guid VoucherId) : ICommand<bool>;
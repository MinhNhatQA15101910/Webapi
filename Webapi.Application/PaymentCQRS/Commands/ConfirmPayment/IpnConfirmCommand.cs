using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Application.Common.Models;
using Webapi.SharedKernel.Enums;

namespace Webapi.Application.PaymentCQRS.Commands.ConfirmPayment;

public class IpnConfirmCommand : ICommand<IpnResponse>
{
    public required object Data { get; set; }
    public required IQueryCollection Query { get; set; }
    public required PaymentMethodEnum PaymentMethod { get; set; }
}

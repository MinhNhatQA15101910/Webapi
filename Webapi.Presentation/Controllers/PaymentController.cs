using MediatR;
using Microsoft.AspNetCore.Mvc;
using Webapi.Application.Common.Models;
using Webapi.Application.Payment.DTOs;
using Webapi.Application.PaymentCQRS.Commands.ConfirmPayment;
using Webapi.Application.PaymentCQRS.Commands.CreatePayment;

namespace Webapi.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController(IMediator mediator) : ControllerBase
{
    // this is for NextJS client call
    [HttpPost("create-payment")]
    public async Task<PaymentResponseDTO> CreatePayment([FromBody] CreatePaymentCommand payload, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(payload, cancellationToken);
        return result;
    }

    // this is VNPay inp url
    [HttpGet("ipn")]
    public async Task<IpnResponse> VNPayIpnConfirm(CancellationToken cancellationToken)
    {
        var command = new IpnConfirmCommand() { Data = Request.Query, Query = Request.Query, PaymentMethod = Webapi.SharedKernel.Enums.PaymentMethodEnum.VNPay };

        var response = await mediator.Send(command, cancellationToken);

        return response;
    }

    // this is Momo inp url
    [HttpPost("ipn-momo")]
    public async Task<IActionResult> MomoIpnConfirm([FromBody]object payload, CancellationToken cancellationToken)
    {
        var command = new IpnConfirmCommand() { Data = Request.Query, Query = Request.Query, PaymentMethod = Webapi.SharedKernel.Enums.PaymentMethodEnum.MoMo };

        var response = await mediator.Send(command, cancellationToken);

        return NoContent();
    }
}

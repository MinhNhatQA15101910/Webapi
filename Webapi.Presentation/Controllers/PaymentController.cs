using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Payment.Services.VnPay;
using Webapi.Application.Common.Models;
using Webapi.Application.Common.Utils.VNPay;
using Webapi.Application.Payment.Commands.CreatePayment;
using Webapi.Application.Payment.DTOs;

namespace Webapi.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController(IMediator mediator) : ControllerBase
{
    // this is for NextJS client call
    [HttpPost("create-payment")]
    public async Task<ActionResult<PaymentResponseDTO>> CreatePayment([FromBody]CreatePaymentDTO payload, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreatePaymentCommand(payload), cancellationToken);
        return Ok(result);
    }

    // this is VNPay inp url
    [HttpGet("ipn")]
    public async Task<IActionResult> VNPayIpnConfirm(CancellationToken cancellationToken)
    {
        if (Request.Query.Count <= 0) return Ok(new IpnResponse("99", "Input data required"));

        try
        {
            var pay = new Provider();
            pay.BuildResponseData(Request.Query);

            bool checkSignature = Utils.CheckSignature(pay, _paymentConfiguration.HashSecret);
            if (!checkSignature)
            {
                return Ok(new IpnResponse("97", "Invalid signature"));
            }

            string orderId = pay.GetResponseData(Parameters.VnpOrderId);
            var order = await GetOrder(orderId, cancellationToken);
            if (order == null) return Ok(new IpnResponse("01", "Order not found"));

            long vnpAmount = Convert.ToInt64(pay.GetResponseData(Parameters.VnpAmount));
            if ((long)(order.TotalAmount * 100) != vnpAmount)
            {
                return Ok(new IpnResponse("04", "Invalid amount"));
            }

            if (order.Status != OrderStatus.Pending) return Ok(new IpnResponse("02", "Order already confirmed"));

            string vnpResponseCode = pay.GetResponseData(Parameters.VnpResponseCode);
            string vnpTransactionStatus = pay.GetResponseData(Parameters.VnpTransactionStatus);

            if (vnpResponseCode == "00" && vnpTransactionStatus == "00")
            {
                //await sendEndpointProvider.SendAsync(
                //    Constants.QueueNames.OnlineStore,
                //    new OrderPaymentSuccessRequest(order.OrderId, ResponseCodes.ResponseCodeDictionary[vnpResponseCode]), cancellationToken);

                return Ok(new IpnResponse("00", "Confirm success"));
            }

            //await sendEndpointProvider.SendAsync(
            //    Constants.QueueNames.OnlineStore,
            //    new OrderPaymentFailedRequest(order.OrderId, ResponseCodes.ResponseCodeDictionary.TryGetValue(vnpResponseCode, out string value) ? value : ResponseCodes.ResponseCodeDictionary["99"]), cancellationToken);

            return Ok(new IpnResponse("00", "Confirm success"));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error when confirm payment");

            return Ok(new IpnResponse("99", "Unknown error"));
        }
    }
}

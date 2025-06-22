using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Web;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Models;
using Webapi.Application.Common.Utils.VNPay;
using Webapi.Application.OrdersCQRS.Commands.CancelOrder;
using Webapi.Application.OrdersCQRS.Commands.ProceedOrder;
using Webapi.Application.Payment.DTOs;
using Webapi.Domain.Enums;
using Webapi.Domain.Interfaces;
using Webapi.Infrastructure.Persistence;
using Webapi.Infrastructure.Services.Configurations;

namespace Webapi.Infrastructure.Services.Services.Payment;

public class VNPayPaymentStrategy(
    AppDbContext dbContext,
    IOptions<VNPaySettings> config,
    IMediator mediator
    ) : IPaymentStrategy
{
    public async Task<PaymentResponseDTO> CreatePaymentAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await dbContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken)
            ?? throw new NotFoundException($"Order {orderId} not found");

        // Access VNPay parameters from configuration
        var vnpUrl = config.Value.BaseUrl;
        var vnpTmnCode = config.Value.TmnCode;
        var vnpReturnUrl = config.Value.ReturnUrl;
        var vnpHashSecret = config.Value.HashSecret;
        var vnpCommand = config.Value.Command;
        var vnpCurrCode = config.Value.CurrCode;
        var vnpVersion = config.Value.Version;
        var vnpLocale = config.Value.Locale;

        // Replace parameters with Plan data
        var amount = order.TotalPrice;
        var orderType = "other"; //trang phục thể thao
        var ipAddress = config.Value.RemoteIpAddress ?? "127.0.0.1"; // Extract IP address

        var hashedOrderId = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}_{order.Id}";

        // Create instance of VnPayLib to handle request URL creation
        var pay = new VNPayProvider();

        // Add parameters to VnPayLib
        pay.AddRequestData(VNPayParameters.VnpVersion, vnpVersion);
        pay.AddRequestData(VNPayParameters.VnpCommand, vnpCommand);
        pay.AddRequestData(VNPayParameters.VnpMerchantCode, vnpTmnCode);
        pay.AddRequestData(VNPayParameters.VnpAmount, ((int)(amount * 26128)).ToString()); // Multiply by 100 to match VNPay amount format

        var createDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));     // Vietnam time
        pay.AddRequestData(VNPayParameters.VnpCreateDate, createDate.ToString("yyyyMMddHHmmss"));

        pay.AddRequestData(VNPayParameters.VnpCurrencyCode, vnpCurrCode);
        pay.AddRequestData(VNPayParameters.VnpIpAddress, ipAddress);
        pay.AddRequestData(VNPayParameters.VnpLanguage, vnpLocale);
        pay.AddRequestData(VNPayParameters.VnpOrderDescription, $"Thanh toan dat hang {orderId}");
        pay.AddRequestData(VNPayParameters.VnpOrderType, orderType!);
        pay.AddRequestData(VNPayParameters.VnpReturnUrl, vnpReturnUrl);

        pay.AddRequestData(VNPayParameters.VnpExpireDate, createDate.AddMinutes(10).ToString("yyyyMMddHHmmss"));

        pay.AddRequestData(VNPayParameters.VnpOrderId, hashedOrderId);

        // Generate the payment URL using VnPayLib
        var payUrl = pay.CreateRequestUrl(vnpUrl, vnpHashSecret);

        Console.WriteLine($"payUrl {payUrl}");

        var result =  new PaymentResponseDTO(orderId, SharedKernel.Enums.PaymentMethodEnum.VNPay, payUrl, 0);
        return result;
    }

    public async Task<IpnResponse> IpnConfirmAsync(object data, IQueryCollection query, CancellationToken cancellationToken = default)
    {
        if (query.Count <= 0) return new IpnResponse("99", "Input data required");

        try
        {
            var pay = new VNPayProvider();
            pay.BuildResponseData(query);

            bool checkSignature = VNPayUtils.CheckSignature(pay, config.Value.HashSecret);
            if (!checkSignature)
            {
                return new IpnResponse("97", "Invalid signature");
            }

            string orderId = pay.GetResponseData(VNPayParameters.VnpOrderId);

            var order = await dbContext.Orders
                .FirstOrDefaultAsync(x => x.Id.ToString() == orderId, cancellationToken);

            if (order == null) return new IpnResponse("01", "Order not found");

            long vnpAmount = Convert.ToInt64(pay.GetResponseData(VNPayParameters.VnpAmount));
            if ((long)(order.TotalPrice * 26128) != vnpAmount)
            {
                return new IpnResponse("04", "Invalid amount");
            }

            if (order.OrderState != OrderStates.Pending.ToString()) 
                return new IpnResponse("02", "Order already confirmed");

            string vnpResponseCode = pay.GetResponseData(VNPayParameters.VnpResponseCode);
            string vnpTransactionStatus = pay.GetResponseData(VNPayParameters.VnpTransactionStatus);

            if (vnpResponseCode == "00" && vnpTransactionStatus == "00")
            {
                //save success state
                await mediator.Send(new ProceedOrderCommand(order.Id), cancellationToken);
                return new IpnResponse("00", "Confirm success");
            }

            //save failed state 
            await mediator.Send(new CancelOrderCommand(order.Id), cancellationToken);
            return new IpnResponse("00", "Confirm success");
        }
        catch (Exception _)
        {
            return new IpnResponse("99", "Unknown error");
        }
    }
}

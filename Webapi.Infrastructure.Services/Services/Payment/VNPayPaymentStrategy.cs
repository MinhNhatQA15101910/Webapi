using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Web;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Payment.DTOs;
using Webapi.Domain.Interfaces;
using Webapi.Infrastructure.Persistence;
using Webapi.Infrastructure.Services.Configurations;
using Webapi.Infrastructure.Services.VNPay;

namespace Webapi.Infrastructure.Services.Services.Payment;

public class VNPayPaymentStrategy(AppDbContext dbContext, IOptions<VNPaySettings> config) : IPaymentStrategy
{
    private readonly HttpClient _httpClient = new HttpClient();

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
        var orderType = "other";
        var ipAddress = config.Value.RemoteIpAddress ?? "127.0.0.1"; // Extract IP address
        var createDate = DateTime.Now;
        var expireDate = createDate.AddMinutes(10);

        // Create instance of VnPayLib to handle request URL creation
        var payLib = new VNPayLib();

        // Add parameters to VnPayLib
        payLib.AddRequestData("vnp_Version", vnpVersion);
        payLib.AddRequestData("vnp_Command", vnpCommand);
        payLib.AddRequestData("vnp_TmnCode", vnpTmnCode);
        payLib.AddRequestData("vnp_Amount", (amount * 100).ToString()); // Multiply by 100 to match VNPay amount format
        payLib.AddRequestData("vnp_CreateDate", createDate.ToString("yyyyMMddHHmmss"));
        payLib.AddRequestData("vnp_CurrCode", vnpCurrCode);
        payLib.AddRequestData("vnp_IpAddr", ipAddress);
        payLib.AddRequestData("vnp_Locale", vnpLocale);
        payLib.AddRequestData("vnp_OrderInfo", HttpUtility.UrlEncode($"Thanh toan dat hang {orderId}"));
        payLib.AddRequestData("vnp_OrderType", orderType!);
        payLib.AddRequestData("vnp_ReturnUrl", vnpReturnUrl);
        payLib.AddRequestData("vnp_ExpireDate", expireDate.ToString("yyyyMMddHHmmss"));
        payLib.AddRequestData("vnp_TxnRef", order.Id.ToString());

        // Generate the payment URL using VnPayLib
        var payUrl =  payLib.CreateRequestUrl(vnpUrl, vnpHashSecret);

        return new PaymentResponseDTO(orderId, SharedKernel.Enums.PaymentEnum.VNPay, payUrl, 0);
    }
}

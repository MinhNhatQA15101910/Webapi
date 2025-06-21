using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Interfaces.Services;
using Webapi.Application.Payment.DTOs;
using Webapi.Domain.Interfaces;
using Webapi.Infrastructure.Persistence;
using Webapi.Infrastructure.Services.Configurations;

namespace Webapi.Infrastructure.Services.Services.Payment;

public class MomoPaymentStrategy(AppDbContext dbContext, IOptions<MomoSettings> config) : IPaymentStrategy
{
    private readonly HttpClient _httpClient = new HttpClient();

    public async Task<PaymentResponseDTO> CreatePaymentAsync(Guid _orderId, CancellationToken cancellationToken = default)
    {
        var order = await dbContext.Orders.FirstOrDefaultAsync(x => x.Id == _orderId, cancellationToken) ?? throw new NotFoundException($"Order {_orderId} not found");

        var url = config.Value.BaseUrl;

        var accessKey = config.Value.AccessKey;
        var secretKey = config.Value.SecretKey;

        // Data for rawSignature
        var amount = order.TotalPrice;
        var orderId = order.Id;
        var orderInfo = order.Id.ToString();
        var requestId = order.Id;

        var storeId = config.Value.StoreId;
        var ipnUrl = config.Value.IpnUrl;
        var extraData = config.Value.ExtraData;
        var partnerCode = config.Value.PartnerCode;
        var redirectUrl = config.Value.RedirectUrl;
        var requestType = config.Value.RequestType;
        var lang = config.Value.Lang;

        var rawSignature = "accessKey=" + accessKey
                                   + "&amount=" + amount
                                   + "&extraData=" + extraData
                                   + "&ipnUrl=" + ipnUrl
                                   + "&orderId=" + orderId
                                   + "&orderInfo=" + orderInfo
                                   + "&partnerCode=" + partnerCode
                                   + "&redirectUrl=" + redirectUrl
                                   + "&requestId=" + requestId
                                   + "&requestType=" + requestType;

        // Payload
        var payload = new
        {
            partnerCode,
            storeId,
            requestId,
            amount,
            orderId,
            orderInfo,
            redirectUrl,
            ipnUrl,
            requestType,
            extraData,
            lang,
            signature = GetSignature(rawSignature, secretKey),
        };

        // Chuyển payload thành JSON
        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Gửi yêu cầu POST
        var response = await _httpClient.PostAsync(url, content);

        // Kiểm tra trạng thái phản hồi
        if (response.IsSuccessStatusCode)
        {
            // Đọc kết quả phản hồi
            var responseContent = await response.Content.ReadAsStringAsync();
            var momoResponse = JsonSerializer.Deserialize<MomoResponse>(responseContent) 
                ?? throw new Application.Common.Exceptions.ApplicationException(
                        "Lỗi hệ thống",
                        "Có lỗi xảy ra khi đọc dữ liệu trả về từ Momo"
                );

            Console.WriteLine(responseContent);
            return new PaymentResponseDTO(orderId, SharedKernel.Enums.PaymentEnum.MoMo, momoResponse.PayUrl, momoResponse.ResultCode);
        }

        Console.WriteLine(response.StatusCode);
        Console.WriteLine(response.Content);

        throw new Application.Common.Exceptions.ApplicationException(
            "Lỗi hệ thống",
            "Có lỗi xảy ra khi sinh PaymentUrl"
        );
    }

    private string GetSignature(String text, String key)
    {
        // change according to your needs, an UTF8Encoding
        // could be more suitable in certain situations
        ASCIIEncoding encoding = new ASCIIEncoding();

        Byte[] textBytes = encoding.GetBytes(text);
        Byte[] keyBytes = encoding.GetBytes(key);

        Byte[] hashBytes;

        using (HMACSHA256 hash = new HMACSHA256(keyBytes))
            hashBytes = hash.ComputeHash(textBytes);

        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }
}

internal class MomoResponse
{
    [JsonPropertyName("partnerCode")]
    public string PartnerCode { get; set; }

    [JsonPropertyName("orderId")]
    public string OrderId { get; set; }

    [JsonPropertyName("requestId")]
    public string RequestId { get; set; }

    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    [JsonPropertyName("responseTime")]
    public long ResponseTime { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("resultCode")]
    public int ResultCode { get; set; }

    [JsonPropertyName("payUrl")]
    public string PayUrl { get; set; }

    [JsonPropertyName("shortLink")]
    public string ShortLink { get; set; }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Webapi.Application.Common.Utils.Momo;

public class MomoCreatePaymentResponseDTO
{
    [JsonPropertyName("partnerCode")]
    public string PartnerCode { get; set; }

    [JsonPropertyName("orderId")]
    public string OrderId { get; set; }

    [JsonPropertyName("requestId")]
    public string RequestId { get; set; }

    [JsonPropertyName("amount")]
    public long Amount { get; set; }

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

public class MomoCreatePaymenyRequestDTO 
{
    [JsonPropertyName("partnerCode")]
    public string PartnerCode { get; set; }

    [JsonPropertyName("storeId")]
    public string StoreId { get; set; }

    [JsonPropertyName("orderId")]
    public string OrderId { get; set; }

    [JsonPropertyName("orderInfo")]
    public string OrderInfo { get; set; }

    [JsonPropertyName("requestId")]
    public string RequestId { get; set; }

    [JsonPropertyName("requestType")]
    public string RequestType { get; set; }

    [JsonPropertyName("amount")]
    public long Amount { get; set; }

    [JsonPropertyName("extraData")]
    public string ExtraData { get; set; }

    [JsonPropertyName("lang")]
    public string Lang { get; set; }

    [JsonPropertyName("signature")]
    public string Signature { get; set; }

    [JsonPropertyName("redirectUrl")]
    public string RedirectUrl { get; set; }

    [JsonPropertyName("ipnUrl")]
    public string IpnUrl { get; set; }
}


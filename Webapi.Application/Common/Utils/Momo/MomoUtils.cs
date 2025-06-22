using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using System.Buffers.Text;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Webapi.Application.Common.Utils.VNPay;

namespace Webapi.Application.Common.Utils.Momo;

public static class MomoUtils
{
    public static string GetSignature(string text, string key)
    {
        // change according to your needs, an UTF8Encoding
        // could be more suitable in certain situations
        ASCIIEncoding encoding = new ();

        Byte[] textBytes = encoding.GetBytes(text);
        Byte[] keyBytes = encoding.GetBytes(key);

        Byte[] hashBytes;

        using (HMACSHA256 hash = new(keyBytes))
            hashBytes = hash.ComputeHash(textBytes);

        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }


    public static bool ValidateSignature(string signature, string text, string key)
    {
        var internalSignature = GetSignature(text, key);
        return internalSignature.Equals(signature, StringComparison.InvariantCultureIgnoreCase);
    }

    public static string GenerateStringPayload(MomoCreatePaymenyRequestDTO dto, string accessKey)
    {
        var rawSignature = "accessKey=" + accessKey
                          + "&amount=" + dto.Amount
                          + "&extraData=" + dto.ExtraData
                          + "&ipnUrl=" + dto.IpnUrl
                          + "&orderId=" + dto.OrderId
                          + "&orderInfo=" + dto.OrderInfo
                          + "&partnerCode=" + dto.PartnerCode
                          + "&redirectUrl=" + dto.RedirectUrl
                          + "&requestId=" + dto.RequestId
                          + "&requestType=" + dto.RequestType;

        return rawSignature;
    }
}

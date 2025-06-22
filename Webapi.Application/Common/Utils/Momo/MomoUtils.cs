using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

    public static string GenerateStringPayload(MomoCreatePaymenyRequestDTO dto)
    {
        var data = new StringBuilder();
        foreach (var kv in dto.GetType().GetProperties())
        {
            var value = kv.GetValue(dto)?.ToString();
            var key = kv.Name;
            if (!string.IsNullOrEmpty(value) && key != null)
            {
                data
                    .Append(WebUtility.UrlEncode(key))
                    .Append('=')
                    .Append(WebUtility.UrlEncode(value))
                    .Append('&');
            }
        }

        string queryString = data.ToString();
        if (queryString.Length > 0)
        {
            queryString = queryString.Remove(data.Length - 1, 1);
        }

        return queryString;
    }
}

using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;

namespace Webapi.Application.Common.Utils.VNPay;

public class VNPayUtils
{
    public static string HmacSha512(string key, string inputData)
    {
        var hash = new StringBuilder();
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
        using (var hmac = new HMACSHA512(keyBytes))
        {
            byte[] hashValue = hmac.ComputeHash(inputBytes);
            foreach (byte theByte in hashValue)
            {
                hash.Append(theByte.ToString("x2"));
            }
        }

        return hash.ToString();
    }

    public static string GetIpAddress(HttpContext context)
    {
        return context.Request.Headers.ContainsKey("X-Forwarded-For")
            ? context.Request.Headers["X-Forwarded-For"].ToString()
            : context.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "N/A";
    }

    public static bool CheckSignature(VNPayProvider pay, string hashSecret)
    {
        string vnpSecureHash = pay.GetResponseData(VNPayParameters.VnpSecureHash);
        return pay.ValidateSignature(vnpSecureHash, hashSecret);
    }
}

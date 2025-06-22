namespace Webapi.Application.Common.Utils.VNPay;

public class VNPayParameters
{
    /// <summary>
    /// Version of VnPay API, current version is 2.1.0.
    /// </summary>
    public const string VnpVersion = "vnp_Version";

    /// <summary>
    /// Command of VnPay API, command for payment transaction is 'pay'.
    /// </summary>
    public const string VnpCommand = "vnp_Command";

    /// <summary>
    /// Merchant website code on VnPay system (when registering an account, it will be in the VnPay email).
    /// </summary>
    public const string VnpMerchantCode = "vnp_TmnCode";

    /// <summary>
    /// Payment amount (in vnd), has to multiply by 100 to eliminate the decimal point.
    /// </summary>
    public const string VnpAmount = "vnp_Amount";

    /// <summary>
    /// Code of the bank that the customer chooses to pay. Ignore this in order for VNPay to handle the payment method selection.
    /// </summary>
    public const string VnpBankCode = "vnp_BankCode";

    /// <summary>
    /// Transaction create date, format: yyyyMMddHHmmss.
    /// </summary>
    public const string VnpCreateDate = "vnp_CreateDate";

    /// <summary>
    /// Currency code, currently only VND is supported.
    /// </summary>
    public const string VnpCurrencyCode = "vnp_CurrCode";

    /// <summary>
    /// IP address of the customer.
    /// </summary>
    public const string VnpIpAddress = "vnp_IpAddr";

    /// <summary>
    /// Language of the payment page: vn/en.
    /// </summary>
    public const string VnpLanguage = "vnp_Locale";

    /// <summary>
    /// Order description.
    /// </summary>
    public const string VnpOrderDescription = "vnp_OrderInfo";

    /// <summary>
    /// Order type, 'other' for online payment.
    /// </summary>
    public const string VnpOrderType = "vnp_OrderType";

    /// <summary>
    /// The return URL after the payment is completed.
    /// </summary>
    public const string VnpReturnUrl = "vnp_ReturnUrl";

    /// <summary>
    /// Expire date.
    /// </summary>
    public const string VnpExpireDate = "vnp_ExpireDate";

    /// <summary>
    /// Order reference number on the merchant system.
    /// </summary>
    public const string VnpOrderId = "vnp_TxnRef";

    /// <summary>
    /// Checksum to ensure the integrity of the data. The checksum is calculated by the HMAC SHA512 algorithm with the hash secret key provided by VnPay.
    /// </summary>
    public const string VnpSecureHash = "vnp_SecureHash";

    /// <summary>
    /// Transaction number on the VNPay system.
    /// </summary>
    public const string VnpTransNo = "vnp_TransactionNo";

    /// <summary>
    /// Transaction status on the VNPay system.
    /// </summary>
    public const string VnpTransactionStatus = "vnp_TransactionStatus";

    /// <summary>
    /// Response code from VNPay. Refer to https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/ for more details.
    /// </summary>
    public const string VnpResponseCode = "vnp_ResponseCode";
}
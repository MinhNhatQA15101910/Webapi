using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webapi.SharedKernel.Enums;

namespace Webapi.Application.Payment.DTOs;

public class PaymentResponseDTO
{
    public Guid OrderId { get; set; }
    public PaymentMethodEnum PaymentMethod { get; set; }
    public string PaymentUrl { get; set; } = string.Empty;
    public int Result { get; set; }
    public PaymentResponseDTO(Guid orderId, PaymentMethodEnum paymentMethod, string paymentUrl, int result)
    {
        OrderId = orderId;
        PaymentMethod = paymentMethod;
        PaymentUrl = paymentUrl;
        Result = result;
    }
}

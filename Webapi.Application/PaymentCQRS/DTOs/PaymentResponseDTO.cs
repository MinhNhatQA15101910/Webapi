using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webapi.SharedKernel.Enums;

namespace Webapi.Application.Payment.DTOs;

public class PaymentResponseDTO
{
    Guid OrderId { get; set; }
    PaymentEnum PaymentMethod { get; set; }
    string PaymentUrl { get; set; } = string.Empty;
    int Result { get; set; }
    public PaymentResponseDTO(Guid orderId, PaymentEnum paymentMethod, string paymentUrl, int result)
    {
        OrderId = orderId;
        PaymentMethod = paymentMethod;
        PaymentUrl = paymentUrl;
        Result = result;
    }
}

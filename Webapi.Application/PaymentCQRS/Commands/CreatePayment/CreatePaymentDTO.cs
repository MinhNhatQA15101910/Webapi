using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webapi.SharedKernel.Enums;

namespace Webapi.Application.Payment.Commands.CreatePayment;

public class CreatePaymentDTO
{
    public Guid orderId;
    public PaymentEnum paymentMethod;
}

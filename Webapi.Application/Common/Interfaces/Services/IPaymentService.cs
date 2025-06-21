using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webapi.Application.Payment.DTOs;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.Enums;

namespace Webapi.Application.Common.Interfaces.Services;

public interface IPaymentService
{
    Task<PaymentResponseDTO> PayAsync(Guid orderId, PaymentEnum paymentMethod, CancellationToken cancellationToken = default);
    Task Confirm(Guid orderId, PaymentEnum paymentMethod, CancellationToken cancellationToken = default);
}

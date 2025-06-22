using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Models;
using Webapi.Application.Payment.DTOs;
using Webapi.SharedKernel.Enums;

namespace Webapi.Application.Common.Interfaces.Services;

public interface IPaymentService
{
    Task<PaymentResponseDTO> PayAsync(Guid orderId, PaymentMethodEnum paymentMethod, CancellationToken cancellationToken = default);
    Task<IpnResponse> Confirm(PaymentMethodEnum paymentMethod, object data, IQueryCollection query, CancellationToken cancellationToken = default);
}

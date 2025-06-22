using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Models;
using Webapi.Application.Payment.DTOs;

namespace Webapi.Domain.Interfaces;

public interface IPaymentStrategy
{
    Task<PaymentResponseDTO> CreatePaymentAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<IpnResponse> IpnConfirmAsync(object data, IQueryCollection query, CancellationToken cancellationToken = default);
}

using Webapi.Application.Payment.DTOs;

namespace Webapi.Domain.Interfaces;

public interface IPaymentStrategy
{
    Task<PaymentResponseDTO> CreatePaymentAsync(Guid orderId, CancellationToken cancellationToken = default);
}

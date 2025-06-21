using Microsoft.Extensions.DependencyInjection;
using Webapi.Application.Common.Interfaces.Services;
using Webapi.Application.Payment.DTOs;
using Webapi.Domain.Interfaces;
using Webapi.Infrastructure.Services.Services.Payment;
using Webapi.SharedKernel.Enums;

namespace Webapi.Infrastructure.Services.Services;

public class PaymentService(IServiceProvider serviceProvider) : IPaymentService
{
    private IPaymentStrategy? _paymentStragtegy;

    public async Task<PaymentResponseDTO> PayAsync(Guid orderId, PaymentEnum paymentMethod, CancellationToken cancellationToken = default)
    {
        SetStrategy(paymentMethod);

        if (_paymentStragtegy != null)
        {
            var response = await _paymentStragtegy.CreatePaymentAsync(orderId, cancellationToken);
            return response;
        }

        throw new InvalidOperationException("Payment strategy is not set. Please set the payment strategy before processing payments.");
    }

    private void SetStrategy(PaymentEnum paymentMethod)
    {
        _paymentStragtegy = paymentMethod switch
        {
            PaymentEnum.MoMo => serviceProvider.GetRequiredService<MomoPaymentStrategy>(),
            _ => throw new NotImplementedException($"Payment method {paymentMethod} not supported")
        };
    }

    public Task Confirm(Guid orderId, PaymentEnum paymentMethod, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

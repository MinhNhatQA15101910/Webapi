using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Webapi.Application.Common.Interfaces.Services;
using Webapi.Application.Common.Models;
using Webapi.Application.Payment.DTOs;
using Webapi.Domain.Interfaces;
using Webapi.Infrastructure.Services.Services.Payment;
using Webapi.SharedKernel.Enums;

namespace Webapi.Infrastructure.Services.Services;

public class PaymentService(IServiceProvider serviceProvider) : IPaymentService
{
    private IPaymentStrategy? _paymentStragtegy;

    public async Task<PaymentResponseDTO> PayAsync(Guid orderId, PaymentMethodEnum paymentMethod, CancellationToken cancellationToken = default)
    {
        SetStrategy(paymentMethod);

        if (_paymentStragtegy != null)
        {
            var response = await _paymentStragtegy.CreatePaymentAsync(orderId, cancellationToken);

            return response;
        }

        throw new InvalidOperationException("Payment strategy is not set. Please set the payment strategy before processing payments.");
    }

    private void SetStrategy(PaymentMethodEnum paymentMethod)
    {


        _paymentStragtegy = paymentMethod switch
        {
            PaymentMethodEnum.MoMo => serviceProvider.GetRequiredService<MomoPaymentStrategy>(),
            PaymentMethodEnum.VNPay => serviceProvider.GetRequiredService<VNPayPaymentStrategy>(),
            _ => throw new NotImplementedException($"Payment method {paymentMethod} not supported")
        };
    }

    public async Task<IpnResponse> Confirm(PaymentMethodEnum paymentMethod, object data, IQueryCollection query, CancellationToken cancellationToken = default)
    {
        SetStrategy(paymentMethod);

        if (_paymentStragtegy != null)
        {
            var response = await _paymentStragtegy.IpnConfirmAsync(data, query, cancellationToken);
            return response;
        }

        throw new NotImplementedException("Payment strategy is not set. Please set the payment strategy before confirming payments.");
    }
}

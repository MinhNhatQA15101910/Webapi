using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Application.Common.Interfaces.Services;
using Webapi.Application.Payment.DTOs;
using Webapi.SharedKernel.Enums;

namespace Webapi.Application.PaymentCQRS.Commands.CreatePayment;

public class CreatePaymentHandler(
    IPaymentService paymentService
) : ICommandHandler<CreatePaymentCommand, PaymentResponseDTO>
{
    public async Task<PaymentResponseDTO> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        return await paymentService.PayAsync(request.OrderId, request.PaymentMethod, cancellationToken);
    }
}

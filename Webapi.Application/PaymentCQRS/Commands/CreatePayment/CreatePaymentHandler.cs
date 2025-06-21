using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Application.Common.Interfaces.Services;
using Webapi.Application.Payment.DTOs;

namespace Webapi.Application.Payment.Commands.CreatePayment;

public class CreatePaymentHandler(
    IPaymentService paymentService
) : ICommandHandler<CreatePaymentCommand, PaymentResponseDTO>
{
    public async Task<PaymentResponseDTO> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        return await paymentService.PayAsync(request.CreatePaymentDTO.orderId, request.CreatePaymentDTO.paymentMethod, cancellationToken);
    }
}

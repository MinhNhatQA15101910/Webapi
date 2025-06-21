using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Application.Payment.DTOs;

namespace Webapi.Application.Payment.Commands.CreatePayment;

public record CreatePaymentCommand(CreatePaymentDTO CreatePaymentDTO) : ICommand<PaymentResponseDTO>;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Application.Payment.DTOs;
using Webapi.SharedKernel.Enums;

namespace Webapi.Application.PaymentCQRS.Commands.CreatePayment;

public record CreatePaymentCommand(Guid OrderId, PaymentMethodEnum PaymentMethod) : ICommand<PaymentResponseDTO>;
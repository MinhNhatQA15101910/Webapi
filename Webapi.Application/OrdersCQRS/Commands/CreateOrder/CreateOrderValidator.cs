using FluentValidation;

namespace Webapi.Application.OrdersCQRS.Commands.CreateOrder;

public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.CreateOrderDto.CartItemIds)
            .NotEmpty()
            .WithMessage("Cart items cannot be empty.");

        RuleFor(x => x.CreateOrderDto.ShippingType)
            .NotEmpty()
            .WithMessage("Shipping type cannot be empty.");

        RuleFor(x => x.CreateOrderDto.ReceiverName)
            .NotEmpty()
            .WithMessage("Receiver name cannot be empty.");

        RuleFor(x => x.CreateOrderDto.ReceiverEmail)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Receiver email must be a valid email address.");

        RuleFor(x => x.CreateOrderDto.DetailAddress)
            .NotEmpty()
            .WithMessage("Detail address cannot be empty.");
    }
}

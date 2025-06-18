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
    }
}

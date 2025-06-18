using FluentValidation;

namespace Webapi.Application.CartItemCQRS.Commands.AddCartItem;

public class AddCartItemValidator : AbstractValidator<AddCartItemCommand>
{
    public AddCartItemValidator()
    {
        RuleFor(x => x.AddCartItemDto.ProductSizeId)
            .NotEmpty()
            .WithMessage("Product size ID is required.");

        RuleFor(x => x.AddCartItemDto.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");
    }
}

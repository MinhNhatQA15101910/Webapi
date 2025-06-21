using FluentValidation;

namespace Webapi.Application.OrdersCQRS.Commands.ProceedOrder;

public class ProceedOrderValidator : AbstractValidator<ProceedOrderCommand>
{
    public ProceedOrderValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("Order ID cannot be empty.")
            .Must(BeAValidGuid)
            .WithMessage("Order ID must be a valid GUID.");
    }

    private bool BeAValidGuid(Guid guid)
    {
        return Guid.TryParse(guid.ToString(), out _);
    }
}

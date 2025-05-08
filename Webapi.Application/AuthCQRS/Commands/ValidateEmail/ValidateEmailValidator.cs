using FluentValidation;

namespace Webapi.Application.AuthCQRS.Commands.ValidateEmail;

public class ValidateEmailValidator : AbstractValidator<ValidateEmailCommand>
{
    public ValidateEmailValidator()
    {
        RuleFor(x => x.ValidateEmailDto.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");
    }
}

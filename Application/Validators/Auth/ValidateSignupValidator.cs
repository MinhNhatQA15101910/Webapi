using Application.Commands.Auth;
using FluentValidation;

namespace Application.Validators.Auth;

public class ValidateSignupValidator : AbstractValidator<ValidateSignupCommand>
{
    public ValidateSignupValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
            .Matches(@"^\S+$").WithMessage("Username must not contain spaces.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters");
    }
}

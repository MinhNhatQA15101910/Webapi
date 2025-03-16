using Application.Commands.Auth;
using FluentValidation;

namespace Application.Validators.Auth;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordValidator()
    {
        RuleFor(x => x.NewPassword)
            .NotNull().WithMessage("New password is required")
            .NotEmpty().WithMessage("New password is required")
            .MinimumLength(8).WithMessage("New password must be at least 8 characters long");
    }
}

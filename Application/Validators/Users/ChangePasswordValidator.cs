using Application.Commands.Users;
using FluentValidation;

namespace Application.Validators.Users;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("CurrentPassword is required.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("NewPassword is required.")
            .MinimumLength(6).WithMessage("NewPassword must be at least 6 characters long.")
            .NotEqual(x => x.CurrentPassword).WithMessage("NewPassword should not be the same as CurrentPassword.");
    }
}

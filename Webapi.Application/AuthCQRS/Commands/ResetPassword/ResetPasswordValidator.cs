using FluentValidation;

namespace Webapi.Application.AuthCQRS.Commands.ResetPassword;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordValidator()
    {
        RuleFor(x => x.ResetPasswordDto.NewPassword)
            .NotEmpty()
            .WithMessage("New password is required.")
            .MinimumLength(6)
            .WithMessage("New password must be at least 6 characters long.");
    }
}

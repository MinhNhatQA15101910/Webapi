using MediatR;

namespace Application.Commands.Auth;

public class ResetPasswordCommand : ICommand<bool>
{
    public Guid? UserId { get; set; }
    public string NewPassword { get; set; } = null!;
}

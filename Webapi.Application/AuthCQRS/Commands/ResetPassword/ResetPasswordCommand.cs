using Webapi.Application.Common.Interfaces.MediatR;

namespace Webapi.Application.AuthCQRS.Commands.ResetPassword;

public record ResetPasswordCommand(ResetPasswordDto ResetPasswordDto) : ICommand<bool>;

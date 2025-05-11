using Webapi.Application.Common.Interfaces.MediatR;

namespace Webapi.Application.UsersCQRS.Commands.ChangePassword;

public record ChangePasswordCommand(ChangePasswordDto ChangePasswordDto) : ICommand<bool>;

using Webapi.Application.Common.Interfaces.MediatR;

namespace Webapi.Application.AuthCQRS.Commands.ValidateEmail;

public record ValidateEmailCommand(ValidateEmailDto ValidateEmailDto) : ICommand<object>;

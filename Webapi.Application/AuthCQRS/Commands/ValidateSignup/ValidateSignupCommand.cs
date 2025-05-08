using Webapi.Application.Common.Interfaces.MediatR;

namespace Webapi.Application.AuthCQRS.Commands.ValidateSignup;

public record ValidateSignupCommand(
    ValidateSignupDto ValidateSignupDto
) : ICommand<string>;

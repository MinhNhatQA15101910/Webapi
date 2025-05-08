using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.AuthCQRS.Commands.Login;

public record LoginCommand(LoginDto LoginDto) : ICommand<UserDto>;

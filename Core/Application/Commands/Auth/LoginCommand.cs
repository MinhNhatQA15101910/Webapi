using Application.DTOs.Auth;
using Application.DTOs.Users;

namespace Application.Commands.Auth;

public record LoginCommand(LoginDto LoginDto) : ICommand<UserDto>;

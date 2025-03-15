using Domain.Dtos;

namespace Application.Commands.Auth;

public record LoginCommand(string Email, string Password) : ICommand<UserDto>;

namespace Application.Commands.Auth;

public record ValidateSignupCommand(
    string Username,
    string Email,
    string Password
) : ICommand<string>;

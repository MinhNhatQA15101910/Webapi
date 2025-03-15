namespace Application.Commands.Auth;

public record ValidateEmailCommand(string Email) : ICommand<object>;

namespace Webapi.Application.AuthCQRS.Commands.ValidateSignup;

public class ValidateSignupDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

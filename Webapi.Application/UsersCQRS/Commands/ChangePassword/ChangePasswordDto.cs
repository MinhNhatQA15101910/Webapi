namespace Webapi.Application.UsersCQRS.Commands.ChangePassword;

public class ChangePasswordDto
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

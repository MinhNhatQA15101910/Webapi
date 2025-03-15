using Application.Helpers;

namespace Application.Commands.Auth;

public class VerifyPincodeCommand : ICommand<object>
{
    public string Pincode { get; set; } = null!;
    public string? Email { get; set; }
    public PincodeAction Action { get; set; }
}

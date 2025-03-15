using System.Security.Claims;
using Application.Helpers;

namespace Web.Extensions;

public static class ClaimPrincipalExtensions
{
    public static string? GetEmail(this ClaimsPrincipal user)
    {
        var email = user.FindFirstValue(ClaimTypes.Email);

        return email;
    }

    public static PincodeAction GetAction(this ClaimsPrincipal user)
    {
        var actionString = user.FindFirstValue("action")
            ?? throw new Exception("Cannot get action from token");

        PincodeAction action = actionString == "Signup"
            ? PincodeAction.Signup
            : actionString == "VerifyEmail"
                ? PincodeAction.VerifyEmail
                : PincodeAction.None;

        return action;
    }
}

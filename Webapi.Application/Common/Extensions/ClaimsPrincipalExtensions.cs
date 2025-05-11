using System.Security.Claims;
using Webapi.Application.Common.Helpers;

namespace Webapi.Application.Common.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var userIdString = user.FindFirstValue(ClaimTypes.NameIdentifier) 
            ?? throw new Exception("Cannot get user id from token");
        return Guid.Parse(userIdString);
    }

    public static string GetEmail(this ClaimsPrincipal user)
    {
        var email = user.FindFirstValue(ClaimTypes.Email) 
            ?? throw new Exception("Cannot get email from token");
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

using Application.Commands.Auth;
using Application.Helpers;
using Domain.Dtos;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginCommand loginCommand)
    {
        var user = await mediator.Send(loginCommand);
        return Ok(user);
    }

    [HttpPost("validate-signup")]
    public async Task<ActionResult<UserDto>> ValidateSignup(ValidateSignupCommand validateSignupCommand)
    {
        var token = await mediator.Send(validateSignupCommand);
        return Ok(new { token });
    }

    [HttpPost("email-exists")]
    public async Task<ActionResult<object>> EmailExists(ValidateEmailCommand validateEmailCommand)
    {
        var exists = await mediator.Send(validateEmailCommand);
        if (exists is bool)
        {
            return exists;
        }

        return Ok(new { token = exists });
    }

    [HttpPost("verify-pincode")]
    [Authorize]
    public async Task<ActionResult<object>> VerifyPincode(VerifyPincodeCommand verifyPincodeCommand)
    {
        // Get email
        var email = User.GetEmail()
            ?? throw new UnauthorizedException("Email not found in claims");

        var action = User.GetAction();
        if (action == PincodeAction.None)
        {
            throw new UnauthorizedException("Invalid action");
        }

        verifyPincodeCommand.Email = email;
        verifyPincodeCommand.Action = action;

        var result = await mediator.Send(verifyPincodeCommand);
        if (result is string)
        {
            return Ok(new { token = result });
        }

        return Ok(result);
    }

    [HttpPatch("reset-password")]
    [Authorize]
    public async Task<ActionResult> ResetPassword(ResetPasswordCommand resetPasswordCommand)
    {
        // Get userId
        var userId = User.GetUserId();

        resetPasswordCommand.UserId = userId;

        await mediator.Send(resetPasswordCommand);

        return NoContent();
    }
}

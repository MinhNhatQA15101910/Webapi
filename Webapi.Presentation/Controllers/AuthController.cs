using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webapi.Application.AuthCQRS.Commands.Login;
using Webapi.Application.AuthCQRS.Commands.ValidateEmail;
using Webapi.Application.AuthCQRS.Commands.ValidateSignup;
using Webapi.Application.AuthCQRS.Commands.VerifyPincode;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await mediator.Send(new LoginCommand(loginDto));
        return Ok(user);
    }

    [HttpPost("validate-signup")]
    public async Task<ActionResult<UserDto>> ValidateSignup(ValidateSignupDto validateSignupDto)
    {
        var token = await mediator.Send(new ValidateSignupCommand(validateSignupDto));
        return Ok(new { token });
    }

    [HttpPost("email-exists")]
    public async Task<ActionResult<object>> EmailExists(ValidateEmailDto validateEmailDto)
    {
        var exists = await mediator.Send(new ValidateEmailCommand(validateEmailDto));
        if (exists is bool)
        {
            return exists;
        }

        return Ok(new { token = exists });
    }

    [HttpPost("verify-pincode")]
    [Authorize]
    public async Task<ActionResult<object>> VerifyPincode(VerifyPincodeDto verifyPincodeDto)
    {
        var result = await mediator.Send(new VerifyPincodeCommand(verifyPincodeDto));
        if (result is string)
        {
            return Ok(new { token = result });
        }

        return Ok(result);
    }
}

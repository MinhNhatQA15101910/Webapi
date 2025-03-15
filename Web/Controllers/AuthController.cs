using Application.Commands.Auth;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
}

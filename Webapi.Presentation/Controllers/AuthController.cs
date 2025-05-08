using MediatR;
using Microsoft.AspNetCore.Mvc;
using Webapi.Application.AuthCQRS.Commands.Login;
using Webapi.Application.AuthCQRS.Commands.ValidateSignup;
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
}

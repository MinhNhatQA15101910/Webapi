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
}

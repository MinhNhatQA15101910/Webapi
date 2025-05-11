using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webapi.Application.UsersCQRS.Queries.GetCurrentUser;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var userDto = await mediator.Send(new GetCurrentUserQuery());
        return Ok(userDto);
    }
}

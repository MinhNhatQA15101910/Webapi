using Application.Queries.Users;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var userId = User.GetUserId();

        var userDto = await mediator.Send(new GetUserByIdQuery(userId));
        return Ok(userDto);
    }
}

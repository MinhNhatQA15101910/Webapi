using Application.Commands.Users;
using Application.Queries.Users;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var userId = User.GetUserId();

        var userDto = await mediator.Send(new GetUserByIdQuery(userId));
        return Ok(userDto);
    }

    [HttpPatch("change-password")]
    [Authorize]
    public async Task<ActionResult> ChangePassword(ChangePasswordCommand changePasswordCommand)
    {
        var userId = User.GetUserId();

        changePasswordCommand.UserId = userId;

        await mediator.Send(changePasswordCommand);

        return NoContent();
    }
}

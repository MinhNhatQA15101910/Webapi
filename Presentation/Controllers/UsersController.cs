using Application.Commands.Users;
using Application.DTOs.Users;
using Application.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;

namespace Presentation.Controllers;

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

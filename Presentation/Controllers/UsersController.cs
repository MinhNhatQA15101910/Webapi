using Application.Commands.Users;
using Application.DTOs.Users;
using Application.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using SharedKernel.DTOs;
using SharedKernel.Params;

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
    public async Task<ActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var userId = User.GetUserId();

        changePasswordDto.UserId = userId;

        await mediator.Send(new ChangePasswordCommand(changePasswordDto));

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery] UserParams userParams)
    {
        userParams.CurrentUserId = User.GetUserId();
        var users = await mediator.Send(new GetUsersQuery(userParams));

        Response.AddPaginationHeader(users);

        return Ok(users);
    }
}

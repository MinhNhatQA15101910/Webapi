using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webapi.Application.Common.Extensions;
using Webapi.Application.UsersCQRS.Commands.AddPhoto;
using Webapi.Application.UsersCQRS.Commands.ChangePassword;
using Webapi.Application.UsersCQRS.Commands.DeletePhoto;
using Webapi.Application.UsersCQRS.Queries.GetCurrentUser;
using Webapi.Application.UsersCQRS.Queries.GetUserById;
using Webapi.Application.UsersCQRS.Queries.GetUsers;
using Webapi.Presentation.Extensions;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.Params;

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

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(Guid id)
    {
        var userDto = await mediator.Send(new GetUserByIdQuery(id));
        return Ok(userDto);
    }

    [HttpPatch("change-password")]
    [Authorize]
    public async Task<ActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        await mediator.Send(new ChangePasswordCommand(changePasswordDto));

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery] UserParams userParams)
    {
        var users = await mediator.Send(new GetUsersQuery(userParams));

        Response.AddPaginationHeader(users);

        return Ok(users);
    }

    [HttpPost("add-photo")]
    [Authorize]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var photo = await mediator.Send(new AddPhotoCommand(file));
        return CreatedAtAction(
            nameof(GetUser),
            new { id = User.GetUserId() },
            photo
        );
    }
    
    [HttpDelete("delete-photo/{photoId}")]
    [Authorize]
    public async Task<IActionResult> DeletePhoto(Guid photoId)
    {
        await mediator.Send(new DeletePhotoCommand(photoId));
        return Ok();
    }
}

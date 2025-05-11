using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Entities;

namespace Webapi.Application.UsersCQRS.Commands.ChangePassword;

public class ChangePasswordHandler(
    IHttpContextAccessor httpContextAccessor,
    UserManager<User> userManager
) : ICommandHandler<ChangePasswordCommand, bool>
{
    public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext.User.GetUserId();

        var user = await userManager.FindByIdAsync(userId.ToString())
            ?? throw new UserNotFoundException(userId);

        user.UpdatedAt = DateTime.UtcNow;
        var changePasswordResult = await userManager.ChangePasswordAsync(
            user,
            request.ChangePasswordDto.CurrentPassword,
            request.ChangePasswordDto.NewPassword
        );
        if (!changePasswordResult.Succeeded)
        {
            throw new IdentityErrorException(changePasswordResult.Errors);
        }

        return true;
    }
}

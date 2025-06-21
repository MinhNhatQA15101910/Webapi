using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Webapi.Application.AuthCQRS.Notifications.UserUpdated;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Entities;

namespace Webapi.Application.AuthCQRS.Commands.ResetPassword;

public class ResetPasswordHandler(
    IHttpContextAccessor httpContextAccessor,
    UserManager<User> userManager,
    IMediator mediator
) : ICommandHandler<ResetPasswordCommand, bool>
{
    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext.User.GetUserId();

        // Get user
        var user = await userManager.FindByIdAsync(userId.ToString())
            ?? throw new UnauthorizedException("User not found");

        // Reset password
        user.UpdatedAt = DateTime.UtcNow;
        user.PasswordHash = userManager.PasswordHasher.HashPassword(user, request.ResetPasswordDto.NewPassword);

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new BadRequestException("Failed to reset password");
        }

        await mediator.Publish(new UserUpdatedNotification(user), cancellationToken);

        return true;
    }
}

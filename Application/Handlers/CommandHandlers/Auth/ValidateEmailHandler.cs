using Application.Commands.Auth;
using Application.Helpers;
using Application.Notifications.Auth;
using Domain.Entities;
using Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.CommandHandlers.Auth;

public class ValidateEmailHandler(
    UserManager<User> userManager,
    PincodeStore pincodeStore,
    ITokenService tokenService,
    IMediator mediator
) : ICommandHandler<ValidateEmailCommand, object>
{
    public async Task<object> Handle(ValidateEmailCommand request, CancellationToken cancellationToken)
    {
        if (!await UserExists(request.Email))
        {
            return false;
        }

        // Add to pincode map
        var pincode = PincodeStore.GeneratePincode();
        pincodeStore.AddPincode(request.Email, pincode);

        // Send pincode email
        await mediator.Publish(
            new EmailValidatedNotification(request.Email, pincode),
            cancellationToken
        );

        return tokenService.CreateVerifyPincodeToken(request.Email, PincodeAction.VerifyEmail.ToString());
    }

    private async Task<bool> UserExists(string email)
    {
        return await userManager.Users.AnyAsync(x => x.NormalizedEmail == email.ToUpper());
    }
}

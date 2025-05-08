using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Webapi.Application.AuthCQRS.Notifications.EmailValidated;
using Webapi.Application.Common.Helpers;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Application.Common.Interfaces.Services;
using Webapi.Domain.Entities;

namespace Webapi.Application.AuthCQRS.Commands.ValidateEmail;

public class ValidateEmailHandler(
    UserManager<User> userManager,
    PincodeStore pincodeStore,
    ITokenService tokenService,
    IMediator mediator
) : ICommandHandler<ValidateEmailCommand, object>
{
    public async Task<object> Handle(ValidateEmailCommand request, CancellationToken cancellationToken)
    {
        if (!await UserExists(request.ValidateEmailDto.Email, cancellationToken))
        {
            return false;
        }

        // Add to pincode map
        var pincode = PincodeStore.GeneratePincode();
        pincodeStore.AddPincode(request.ValidateEmailDto.Email, pincode);

        // Send pincode email
        await mediator.Publish(
            new EmailValidatedNotification(request.ValidateEmailDto.Email, pincode),
            cancellationToken
        );

        return tokenService.CreateVerifyPincodeToken(request.ValidateEmailDto.Email, PincodeAction.VerifyEmail.ToString());
    }

    private async Task<bool> UserExists(string email, CancellationToken cancellationToken)
    {
        return await userManager.Users.AnyAsync(x => x.NormalizedEmail == email.ToUpper(), cancellationToken: cancellationToken);
    }
}

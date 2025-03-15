using Application.Commands.Auth;
using Application.Helpers;
using Application.Notifications.Auth;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.CommandHandlers.Auth;

public class ValidateSignupHandler(
    UserManager<User> userManager,
    PincodeStore pincodeStore,
    ITokenService tokenService,
    IMediator mediator
) : ICommandHandler<ValidateSignupCommand, string>
{
    public async Task<string> Handle(ValidateSignupCommand request, CancellationToken cancellationToken)
    {
        // Check if email already exists
        if (await UserExists(request.Email))
        {
            throw new BadRequestException("Email already exists.");
        }

        // Check if password is valid
        var result = await userManager.PasswordValidators.First().ValidateAsync(
            userManager,
            null!,
            request.Password
        );

        if (!result.Succeeded)
        {
            throw new IdentityErrorException(result.Errors);
        }

        // Add to pincode map
        var pincode = PincodeStore.GeneratePincode();
        pincodeStore.AddPincode(request.Email, pincode);

        // Add to validate user map
        pincodeStore.AddValidateUser(request.Email, request);

        // Send pincode email
        await mediator.Publish(
            new SignupValidatedNotification(request.Username, request.Email, pincode),
            cancellationToken
        );

        return tokenService.CreateVerifyPincodeToken(request.Email, PincodeAction.Signup.ToString());
    }

    private async Task<bool> UserExists(string email)
    {
        return await userManager.Users.AnyAsync(x => x.NormalizedEmail == email.ToUpper());
    }
}

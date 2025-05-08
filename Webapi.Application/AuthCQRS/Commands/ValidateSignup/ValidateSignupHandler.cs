using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Webapi.Application.AuthCQRS.Notifications.SignupValidated;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Helpers;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Application.Common.Interfaces.Services;
using Webapi.Domain.Entities;

namespace Webapi.Application.AuthCQRS.Commands.ValidateSignup;

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
        if (await UserExists(request.ValidateSignupDto.Email))
        {
            throw new BadRequestException("Email already exists.");
        }

        // Check if password is valid
        var result = await userManager.PasswordValidators.First().ValidateAsync(
            userManager,
            null!,
            request.ValidateSignupDto.Password
        );

        if (!result.Succeeded)
        {
            throw new IdentityErrorException(result.Errors);
        }

        // Add to pincode map
        var pincode = PincodeStore.GeneratePincode();
        pincodeStore.AddPincode(request.ValidateSignupDto.Email, pincode);

        // Add to validate user map
        pincodeStore.AddValidateUser(request.ValidateSignupDto.Email, request.ValidateSignupDto);

        // Send pincode email
        await mediator.Publish(
            new SignupValidatedNotification(request.ValidateSignupDto.Username, request.ValidateSignupDto.Email, pincode),
            cancellationToken
        );

        return tokenService.CreateVerifyPincodeToken(request.ValidateSignupDto.Email, PincodeAction.Signup.ToString());
    }

    private async Task<bool> UserExists(string email)
    {
        return await userManager.Users.AnyAsync(x => x.NormalizedEmail == email.ToUpper());
    }
}

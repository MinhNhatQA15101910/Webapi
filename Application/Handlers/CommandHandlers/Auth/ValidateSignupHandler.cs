using Application.Commands.Auth;
using Application.Helpers;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.CommandHandlers.Auth;

public class ValidateSignupHandler(
    UserManager<User> userManager,
    PincodeStore pincodeStore,
    ITokenService tokenService
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
        // var displayName = registerDto.FirstName;
        // var email = registerDto.Email;
        // var subject = "VERA ACCOUNT VERIFICATION CODE";
        // var message = await System.IO.File.ReadAllTextAsync("./Assets/EmailContent.html");
        // message = message.Replace("{{hideEmail}}", HideEmail(email));
        // message = message.Replace("{{pincode}}", pincode);

        // await emailService.SendEmailAsync(
        //     new EmailMessage(
        //         displayName,
        //         email,
        //         subject,
        //         message
        //     )
        // );

        return tokenService.CreateVerifyPincodeToken(request.Email, PincodeAction.Signup.ToString());
    }

    private async Task<bool> UserExists(string email)
    {
        return await userManager.Users.AnyAsync(x => x.NormalizedEmail == email.ToUpper());
    }
}

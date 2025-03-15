using Application.Commands.Auth;
using Application.Helpers;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Services;
using Microsoft.AspNetCore.Identity;

namespace Application.Handlers.CommandHandlers.Auth;

public class VerifyPincodeHandler(
    PincodeStore pincodeStore,
    IMapper mapper,
    UserManager<User> userManager,
    ITokenService tokenService
) : ICommandHandler<VerifyPincodeCommand, object>
{
    public async Task<object> Handle(VerifyPincodeCommand request, CancellationToken cancellationToken)
    {
        var pincode = pincodeStore.GetPincode(request.Email!);
        if (pincode != request.Pincode)
        {
            throw new BadRequestException("Incorrect pincode");
        }

        // Remove pincode
        pincodeStore.RemovePincode(request.Email!);

        // Process action
        if (request.Action == PincodeAction.Signup)
        {
            var validateSignupCommand = pincodeStore.GetValidateUser(request.Email!);
            var user = mapper.Map<User>(validateSignupCommand);

            var result = await userManager.CreateAsync(user, validateSignupCommand.Password);
            if (!result.Succeeded)
            {
                throw new IdentityErrorException(result.Errors);
            }

            pincodeStore.RemoveValidateUser(request.Email!);

            var roleResult = await userManager.AddToRoleAsync(user, "User");
            if (!roleResult.Succeeded)
            {
                throw new IdentityErrorException(roleResult.Errors);
            }

            var userDto = mapper.Map<UserDto>(user);
            userDto.Token = await tokenService.CreateTokenAsync(user);

            return userDto;
        }
        else if (request.Action == PincodeAction.VerifyEmail)
        {
            var user = await userManager.FindByEmailAsync(request.Email!)
                ?? throw new UnauthorizedException("User not found");
            
            return await tokenService.CreateTokenAsync(user);
        }

        throw new BadRequestException("Invalid action");
    }
}

using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Webapi.Application.AuthCQRS.Notifications.UserAdded;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Helpers;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Application.Common.Interfaces.Services;
using Webapi.Domain.Entities;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.AuthCQRS.Commands.VerifyPincode;

public class VerifyPincodeHandler(
    PincodeStore pincodeStore,
    IMapper mapper,
    UserManager<User> userManager,
    ITokenService tokenService,
    IHttpContextAccessor httpContextAccessor,
    IMediator mediator
) : ICommandHandler<VerifyPincodeCommand, object>
{
    public async Task<object> Handle(VerifyPincodeCommand request, CancellationToken cancellationToken)
    {
        var email = httpContextAccessor.HttpContext?.User.GetEmail();
        var action = httpContextAccessor.HttpContext?.User.GetAction();

        var pincode = pincodeStore.GetPincode(email!);
        if (pincode != request.VerifyPincodeDto.Pincode)
        {
            throw new BadRequestException("Incorrect pincode");
        }

        // Remove pincode
        pincodeStore.RemovePincode(email!);

        // Process action
        if (action == PincodeAction.Signup)
        {
            var validateSignupDto = pincodeStore.GetValidateUser(email!);
            var user = mapper.Map<User>(validateSignupDto);

            var result = await userManager.CreateAsync(user, validateSignupDto.Password);
            if (!result.Succeeded)
            {
                throw new IdentityErrorException(result.Errors);
            }

            pincodeStore.RemoveValidateUser(email!);

            var roleResult = await userManager.AddToRoleAsync(user, "User");
            if (!roleResult.Succeeded)
            {
                throw new IdentityErrorException(roleResult.Errors);
            }

            await mediator.Publish(new UserAddedNotification(), cancellationToken);

            var userDto = mapper.Map<UserDto>(user);
            userDto.Token = await tokenService.CreateTokenAsync(user);

            return userDto;
        }
        else if (action == PincodeAction.VerifyEmail)
        {
            var user = await userManager.FindByEmailAsync(email!)
                ?? throw new UnauthorizedException("User not found");

            return await tokenService.CreateTokenAsync(user);
        }

        throw new BadRequestException("Invalid action");
    }
}

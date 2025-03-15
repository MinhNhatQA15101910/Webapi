using Application.Commands.Auth;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.CommandHandlers.Auth;

public class LoginHandler(
    UserManager<User> userManager,
    IMapper mapper,
    ITokenService tokenService
) : ICommandHandler<LoginCommand, UserDto>
{
    public async Task<UserDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await userManager.Users
            .Include(x => x.Photos)
            .Include(x => x.UserRoles).ThenInclude(x => x.Role)
            .SingleOrDefaultAsync(x => x.NormalizedEmail == request.Email.ToUpper(), cancellationToken)
            ?? throw new UnauthorizedException("User with this email does not exist.");

        var result = await userManager.CheckPasswordAsync(existingUser, request.Password);
        if (!result)
        {
            throw new UnauthorizedException("Invalid password.");
        }

        var userDto = mapper.Map<UserDto>(existingUser);
        userDto.Token = await tokenService.CreateTokenAsync(existingUser);

        return userDto;
    }
}

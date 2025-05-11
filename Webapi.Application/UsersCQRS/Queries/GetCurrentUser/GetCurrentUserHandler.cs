using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.UsersCQRS.Queries.GetCurrentUser;

public class GetCurrentUserHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IQueryHandler<GetCurrentUserQuery, UserDto>
{
    public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext.User.GetUserId();

        var user = await unitOfWork.UserRepository
            .GetUserByIdAsync(userId, cancellationToken)
            ?? throw new UserNotFoundException(userId);

        return mapper.Map<UserDto>(user);
    }
}

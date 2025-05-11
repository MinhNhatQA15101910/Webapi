using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.Helpers;

namespace Webapi.Application.UsersCQRS.Queries.GetUsers;

public class GetUsersHandler(
    IHttpContextAccessor httpContextAccessor,
    IUserRepository userRepository
) : IQueryHandler<GetUsersQuery, PagedList<UserDto>>
{
    public Task<PagedList<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = httpContextAccessor.HttpContext.User.GetUserId();

        return userRepository.GetUsersAsync(currentUserId, request.UserParams, cancellationToken);
    }
}

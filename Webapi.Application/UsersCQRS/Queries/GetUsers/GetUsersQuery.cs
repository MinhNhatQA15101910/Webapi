using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.Helpers;
using Webapi.SharedKernel.Params;

namespace Webapi.Application.UsersCQRS.Queries.GetUsers;

public record GetUsersQuery(UserParams UserParams) : IQuery<PagedList<UserDto>>;

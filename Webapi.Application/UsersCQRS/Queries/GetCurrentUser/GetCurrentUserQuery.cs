using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.UsersCQRS.Queries.GetCurrentUser;

public record GetCurrentUserQuery : IQuery<UserDto>;

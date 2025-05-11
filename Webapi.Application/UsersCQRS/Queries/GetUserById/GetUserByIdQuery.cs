using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.UsersCQRS.Queries.GetUserById;

public record GetUserByIdQuery(Guid Id) : IQuery<UserDto>;

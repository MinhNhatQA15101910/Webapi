using Application.DTOs.Users;

namespace Application.Queries.Users;

public record GetUserByIdQuery(Guid Id) : IQuery<UserDto>;

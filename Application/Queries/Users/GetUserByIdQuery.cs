using Domain.Dtos;

namespace Application.Queries.Users;

public record GetUserByIdQuery(Guid Id) : IQuery<UserDto>;

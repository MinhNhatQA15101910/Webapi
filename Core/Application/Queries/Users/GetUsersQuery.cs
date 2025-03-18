using MediatR;
using SharedKernel;
using SharedKernel.DTOs;
using SharedKernel.Params;

namespace Application.Queries.Users;

public record GetUsersQuery(UserParams UserParams) : IRequest<PagedList<UserDto>>;

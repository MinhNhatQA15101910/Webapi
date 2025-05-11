using AutoMapper;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.UsersCQRS.Queries.GetUserById;

public class GetUserByIdHandler(
    IUserRepository userRepository,
    IMapper mapper
) : IQueryHandler<GetUserByIdQuery, UserDto>
{
    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository
            .GetUserByIdAsync(request.Id, cancellationToken)
            ?? throw new UserNotFoundException(request.Id);

        return mapper.Map<UserDto>(user);
    }
}

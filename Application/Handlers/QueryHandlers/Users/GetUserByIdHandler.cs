using Application.Queries.Users;
using AutoMapper;
using Domain.Dtos;
using Domain.Exceptions;
using Domain.Repositories;

namespace Application.Handlers.QueryHandlers.Users;

public class GetUserByIdHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IQueryHandler<GetUserByIdQuery, UserDto>
{
    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.UserRepository.GetUserByIdAsync(request.Id)
            ?? throw new UserNotFoundException(request.Id);

        return mapper.Map<UserDto>(user);
    }
}

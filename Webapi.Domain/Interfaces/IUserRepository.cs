using Webapi.Domain.Entities;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.Helpers;
using Webapi.SharedKernel.Params;

namespace Webapi.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedList<UserDto>> GetUsersAsync(Guid userId, UserParams userParams, CancellationToken cancellationToken = default);
}

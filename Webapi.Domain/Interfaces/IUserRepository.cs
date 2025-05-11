using Webapi.Domain.Entities;

namespace Webapi.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
}

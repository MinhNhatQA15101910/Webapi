using Microsoft.EntityFrameworkCore;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;

namespace Webapi.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .Include(u => u.Photos)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }
}

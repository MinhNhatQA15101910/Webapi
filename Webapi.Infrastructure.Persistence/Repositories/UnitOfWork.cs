using Webapi.Domain.Interfaces;

namespace Webapi.Infrastructure.Persistence.Repositories;

public class UnitOfWork(
    AppDbContext dbContext, 
    IUserRepository userRepository
) : IUnitOfWork
{
    public IUserRepository UserRepository => userRepository;

    public async Task<bool> CompleteAsync()
    {
        return await dbContext.SaveChangesAsync() > 0;
    }
}

using Webapi.Domain.Interfaces;

namespace Webapi.Infrastructure.Persistence.Repositories;

public class UnitOfWork(
    AppDbContext dbContext, 
    IUserRepository userRepository,
    IProductRepository productRepository
) : IUnitOfWork
{
    public IUserRepository UserRepository => userRepository;
    public IProductRepository ProductRepository => productRepository;

    public async Task<bool> CompleteAsync()
    {
        return await dbContext.SaveChangesAsync() > 0;
    }
}

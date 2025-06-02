using Webapi.Domain.Interfaces;

namespace Webapi.Infrastructure.Persistence.Repositories;

public class UnitOfWork(
    AppDbContext dbContext, 
    IUserRepository userRepository,
    IProductRepository productRepository,
    IProductPhotoRepository productPhotoRepository,
    ICategoryRepository categoryRepository,
    IProductSizeRepository productSizeRepository
) : IUnitOfWork
{
    public IUserRepository UserRepository => userRepository;
    public IProductRepository ProductRepository => productRepository;
    
    public ICategoryRepository CategoryRepository => categoryRepository;
    public IProductPhotoRepository ProductPhotoRepository => productPhotoRepository;
    public IProductSizeRepository ProductSizeRepository => productSizeRepository;

    public async Task<bool> CompleteAsync()
    {
        return await dbContext.SaveChangesAsync() > 0;
    }
}

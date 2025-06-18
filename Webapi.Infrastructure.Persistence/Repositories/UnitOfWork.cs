using Webapi.Domain.Interfaces;

namespace Webapi.Infrastructure.Persistence.Repositories;

public class UnitOfWork(
    AppDbContext dbContext,
    IUserRepository userRepository,
    IProductRepository productRepository,
    IProductPhotoRepository productPhotoRepository,
    ICategoryRepository categoryRepository,
    IProductSizeRepository productSizeRepository,
    ICartItemRepository cartItemRepository,
    IOrderRepository orderRepository
) : IUnitOfWork
{
    public IUserRepository UserRepository => userRepository;
    public IProductRepository ProductRepository => productRepository;

    public ICategoryRepository CategoryRepository => categoryRepository;
    public IProductPhotoRepository ProductPhotoRepository => productPhotoRepository;
    public IProductSizeRepository ProductSizeRepository => productSizeRepository;
    public ICartItemRepository CartItemRepository => cartItemRepository;
    public IOrderRepository OrderRepository => orderRepository;

    public async Task<bool> CompleteAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}

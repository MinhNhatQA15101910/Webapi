namespace Webapi.Domain.Interfaces;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IProductRepository ProductRepository { get; }
    IProductPhotoRepository ProductPhotoRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    IProductSizeRepository ProductSizeRepository { get; }
    ICartItemRepository CartItemRepository { get; }
    IOrderRepository OrderRepository { get; }
    IVoucherRepository VoucherRepository { get; }
    IVoucherItemRepository VoucherItemRepository { get; } // Add the new repository
    IReviewRepository ReviewRepository { get; }
    
    Task<bool> CompleteAsync(CancellationToken cancellationToken = default);
}

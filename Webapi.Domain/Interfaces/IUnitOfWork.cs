namespace Webapi.Domain.Interfaces;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IProductRepository ProductRepository { get; }
    IProductPhotoRepository ProductPhotoRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    IProductSizeRepository ProductSizeRepository { get; }
    
    Task<bool> CompleteAsync();
}

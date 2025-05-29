using System.Linq.Expressions;
using Webapi.Domain.Entities;
using Webapi.SharedKernel.Helpers;
using Webapi.SharedKernel.Params;

namespace Webapi.Domain.Interfaces;

public interface IProductRepository
{
    // Basic CRUD operations
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Product?> GetProductWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    
    // Filtered queries
    Task<PagedList<Product>> GetProductsAsync(ProductParams productParams, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> FindAsync(Expression<Func<Product, bool>> predicate, CancellationToken cancellationToken = default);
    
    // Create/Update/Delete
    void Add(Product product);
    void Update(Product product);
    void Delete(Product product);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    
    // Category operations
    Task AddCategoryAsync(Guid productId, Guid categoryId, CancellationToken cancellationToken = default);
    Task RemoveCategoryAsync(Guid productId, Guid categoryId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Category>> GetProductCategoriesAsync(Guid productId, CancellationToken cancellationToken = default);
    
    // Photo operations
    Task<ProductPhoto?> GetPhotoByIdAsync(Guid photoId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductPhoto>> GetProductPhotosAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<ProductPhoto?> GetMainPhotoAsync(Guid productId, CancellationToken cancellationToken = default);
    Task AddPhotoAsync(Guid productId, ProductPhoto photo, CancellationToken cancellationToken = default);
    Task SetMainPhotoAsync(Guid productId, Guid photoId, CancellationToken cancellationToken = default);
    Task DeletePhotoAsync(Guid photoId, CancellationToken cancellationToken = default);
    
    // Inventory operations
    Task UpdateStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default);
    Task<bool> IsInStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default);
    
    // Search operations
    Task<PagedList<Product>> SearchProductsAsync(string searchTerm, ProductParams productParams, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetFeaturedProductsAsync(int count, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetNewArrivalsAsync(int count, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetRelatedProductsAsync(Guid productId, int count, CancellationToken cancellationToken = default);
}
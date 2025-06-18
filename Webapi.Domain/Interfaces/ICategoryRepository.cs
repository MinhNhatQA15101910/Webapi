using System.Linq.Expressions;
using Webapi.Domain.Entities;
using Webapi.SharedKernel.Helpers;
using Webapi.SharedKernel.Params;

namespace Webapi.Domain.Interfaces;

public interface ICategoryRepository
{
    // Basic CRUD operations
    Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Category?> GetCategoryWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default);

    // Filtered queries
    Task<PagedList<Category>> GetCategoriesAsync(CategoryParams categoryParams, CancellationToken cancellationToken = default);
    Task<IEnumerable<Category>> FindAsync(Expression<Func<Category, bool>> predicate, CancellationToken cancellationToken = default);

    // Create/Update/Delete
    void Add(Category category);
    void Update(Category category);
    void Delete(Category category);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

    // Product operations
    Task<bool> HasProductsAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetCategoryProductsAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);
}

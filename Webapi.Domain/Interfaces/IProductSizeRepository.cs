using Webapi.Domain.Entities;
using Webapi.SharedKernel.Helpers;
using Webapi.SharedKernel.Params;

namespace Webapi.Domain.Interfaces;

public interface IProductSizeRepository
{
    Task<ProductSize?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductSize>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductSize>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<PagedList<ProductSize>> GetProductSizesAsync(ProductSizeParams productSizeParams, CancellationToken cancellationToken = default);
    void Add(ProductSize productSize);
    void Update(ProductSize productSize);
    void Remove(ProductSize productSize);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);
}

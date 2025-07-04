using Microsoft.EntityFrameworkCore;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.Helpers;
using Webapi.SharedKernel.Params;

namespace Webapi.Infrastructure.Persistence.Repositories;

public class ProductSizeRepository(AppDbContext context) : IProductSizeRepository
{
    public async Task<ProductSize?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.ProductSizes
            .Include(ps => ps.Product).ThenInclude(p => p.Photos)
            .FirstOrDefaultAsync(ps => ps.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<ProductSize>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.ProductSizes
            .Include(ps => ps.Product)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductSize>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await context.ProductSizes
            .Where(ps => ps.ProductId == productId)
            .ToListAsync(cancellationToken);
    }

    public async Task<PagedList<ProductSize>> GetProductSizesAsync(ProductSizeParams productSizeParams, CancellationToken cancellationToken = default)
    {
        var query = context.ProductSizes
            .Include(ps => ps.Product)
            .AsQueryable();

        // Apply filtering
        if (!string.IsNullOrEmpty(productSizeParams.Size))
        {
            query = query.Where(ps => ps.Size.Contains(productSizeParams.Size));
        }

        if (productSizeParams.ProductId.HasValue)
        {
            query = query.Where(ps => ps.ProductId == productSizeParams.ProductId.Value);
        }

        // Apply sorting
        query = productSizeParams.OrderBy?.ToLower() switch
        {
            "size" => productSizeParams.SortBy?.ToLower() == "desc"
                ? query.OrderByDescending(ps => ps.Size)
                : query.OrderBy(ps => ps.Size),
            "quantity" => productSizeParams.SortBy?.ToLower() == "desc"
                ? query.OrderByDescending(ps => ps.Quantity)
                : query.OrderBy(ps => ps.Quantity),
            "created" => productSizeParams.SortBy?.ToLower() == "desc"
                ? query.OrderByDescending(ps => ps.CreatedAt)
                : query.OrderBy(ps => ps.CreatedAt),
            _ => productSizeParams.SortBy?.ToLower() == "desc"
                ? query.OrderByDescending(ps => ps.Size)
                : query.OrderBy(ps => ps.Size),
        };

        return await PagedList<ProductSize>.CreateAsync(
            query,
            productSizeParams.PageNumber,
            productSizeParams.PageSize,
            cancellationToken
        );
    }

    public void Add(ProductSize productSize)
    {
        context.ProductSizes.Add(productSize);
    }

    public void Update(ProductSize productSize)
    {
        context.Entry(productSize).State = EntityState.Modified;
    }

    public void Remove(ProductSize productSize)
    {
        context.ProductSizes.Remove(productSize);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.ProductSizes.AnyAsync(ps => ps.Id == id, cancellationToken);
    }

    public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return await context.ProductSizes.AnyAsync(cancellationToken);
    }
}

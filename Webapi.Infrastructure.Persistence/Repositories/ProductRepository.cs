using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.Helpers;
using Webapi.SharedKernel.Params;

namespace Webapi.Infrastructure.Persistence.Repositories;

public class ProductRepository(AppDbContext context) : IProductRepository
{
    private readonly AppDbContext _context = context;

    #region Basic CRUD Operations

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<Product?> GetProductWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Include(p => p.Photos)
            .Include(p => p.Categories)
                .ThenInclude(pc => pc.Category)
            .Include(p => p.Sizes)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Include(p => p.Photos)
            .Include(p => p.Categories)
                .ThenInclude(pc => pc.Category)
            .Include(p => p.Sizes) // Add this line to include sizes
            .ToListAsync(cancellationToken);
    }

    public async Task<PagedList<Product>> GetProductsAsync(ProductParams productParams, CancellationToken cancellationToken = default)
    {
        var query = _context.Products
            .Include(p => p.Photos)
            .Include(p => p.Categories)
                .ThenInclude(pc => pc.Category)
            .Include(p => p.Sizes)
            .AsQueryable();

        // Apply filtering
        if (!string.IsNullOrEmpty(productParams.Name))
        {
            query = query.Where(p => p.Name.Contains(productParams.Name));
        }

        if (!string.IsNullOrEmpty(productParams.Category))
        {
            query = query.Where(p => p.Categories.Any(pc => pc.Category.Name.Contains(productParams.Category)));
        }

        if (productParams.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= productParams.MinPrice.Value);
        }

        if (productParams.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= productParams.MaxPrice.Value);
        }

        // Convert the query to list before ordering by price
        // This moves the sorting operation to the client side for decimal fields
        if (productParams.OrderBy?.ToLower() == "price")
        {
            // Execute the query to get the data without ordering
            var products = await query.ToListAsync(cancellationToken);

            // Then sort in memory
            if (productParams.SortBy?.ToLower() == "desc")
            {
                products = products.OrderByDescending(p => p.Price).ToList();
            }
            else
            {
                products = products.OrderBy(p => p.Price).ToList();
            }

            // Create paged list from memory collection
            return PagedList<Product>.Create(
                products,
                productParams.PageNumber,
                productParams.PageSize
            );
        }
        else
        {
            // For other fields, let the database handle the sorting
            query = productParams.OrderBy?.ToLower() switch
            {
                "name" => productParams.SortBy?.ToLower() == "desc"
                    ? query.OrderByDescending(p => p.Name)
                    : query.OrderBy(p => p.Name),
                "created" => productParams.SortBy?.ToLower() == "desc"
                    ? query.OrderByDescending(p => p.CreatedAt)
                    : query.OrderBy(p => p.CreatedAt),
                _ => productParams.SortBy?.ToLower() == "desc"
                    ? query.OrderByDescending(p => p.Name)
                    : query.OrderBy(p => p.Name),
            };

            return await PagedList<Product>.CreateAsync(
                query,
                productParams.PageNumber,
                productParams.PageSize,
                cancellationToken
            );
        }
    }

    public async Task<IEnumerable<Product>> FindAsync(Expression<Func<Product, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Include(p => p.Photos)
            .Include(p => p.Categories)
                .ThenInclude(pc => pc.Category)
            .Include(p => p.Sizes) // Add this line to include sizes
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public void Add(Product product)
    {
        _context.Products.Add(product);
    }

    public void Update(Product product)
    {
        product.UpdatedAt = DateTime.UtcNow;
        _context.Products.Update(product);
    }

    public void Delete(Product product)
    {
        _context.Products.Remove(product);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Products.AnyAsync(p => p.Id == id, cancellationToken);
    }

    #endregion

    #region Category Operations

    public async Task AddCategoryAsync(Guid productId, Guid categoryId, CancellationToken cancellationToken = default)
    {
        var productCategory = new ProductCategory
        {
            ProductId = productId,
            CategoryId = categoryId,
            CreatedAt = DateTime.UtcNow
        };

        _context.ProductCategories.Add(productCategory);
        // Remove SaveChangesAsync
    }

    public async Task RemoveCategoryAsync(Guid productId, Guid categoryId, CancellationToken cancellationToken = default)
    {
        var productCategory = await _context.ProductCategories
            .FirstOrDefaultAsync(pc => pc.ProductId == productId && pc.CategoryId == categoryId, cancellationToken);

        if (productCategory != null)
        {
            _context.ProductCategories.Remove(productCategory);
            // Remove SaveChangesAsync
        }
    }

    public async Task<IEnumerable<Category>> GetProductCategoriesAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _context.ProductCategories
            .Where(pc => pc.ProductId == productId)
            .Select(pc => pc.Category)
            .ToListAsync(cancellationToken);
    }

    #endregion

    #region Photo Operations

    public async Task<ProductPhoto?> GetPhotoByIdAsync(Guid photoId, CancellationToken cancellationToken = default)
    {
        return await _context.ProductPhotos
            .FirstOrDefaultAsync(p => p.Id == photoId, cancellationToken);
    }

    public async Task<IEnumerable<ProductPhoto>> GetProductPhotosAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        // Missing the filter by productId
        return await _context.ProductPhotos
            .Where(p => p.ProductId == productId)  // Add this filter
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductPhoto?> GetMainPhotoAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _context.ProductPhotos
            .FirstOrDefaultAsync(p => p.ProductId == productId && p.IsMain, cancellationToken);
    }

    public async Task AddPhotoAsync(Guid productId, ProductPhoto photo, CancellationToken cancellationToken = default)
    {
        // Set the ProductId
        photo.ProductId = productId;

        // If this is the first photo, make it main
        if (!await _context.ProductPhotos.AnyAsync(p => p.ProductId == productId, cancellationToken))
        {
            photo.IsMain = true;
        }

        _context.ProductPhotos.Add(photo);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SetMainPhotoAsync(Guid productId, Guid photoId, CancellationToken cancellationToken = default)
    {
        var currentMainPhoto = await _context.ProductPhotos
            .FirstOrDefaultAsync(p => p.ProductId == productId && p.IsMain, cancellationToken);

        if (currentMainPhoto != null)
        {
            currentMainPhoto.IsMain = false;
        }

        var newMainPhoto = await _context.ProductPhotos
            .FirstOrDefaultAsync(p => p.Id == photoId, cancellationToken);

        if (newMainPhoto != null)
        {
            newMainPhoto.IsMain = true;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task DeletePhotoAsync(Guid photoId, CancellationToken cancellationToken = default)
    {
        var photo = await _context.ProductPhotos
            .FirstOrDefaultAsync(p => p.Id == photoId, cancellationToken);

        if (photo != null)
        {
            // Don't allow deleting the main photo if there are other photos
            if (photo.IsMain)
            {
                var otherPhotos = await _context.ProductPhotos
                    .Where(p => p.ProductId == photo.ProductId && p.Id != photoId)
                    .ToListAsync(cancellationToken);

                if (otherPhotos.Any())
                {
                    var newMainPhoto = otherPhotos.First();
                    newMainPhoto.IsMain = true;
                }
            }

            _context.ProductPhotos.Remove(photo);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    #endregion

    #region Inventory Operations

    public async Task UpdateStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default)
    {
        var product = await _context.Products.FindAsync(new object[] { productId }, cancellationToken);

        if (product != null)
        {
            product.InStock = quantity;
            product.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> IsInStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default)
    {
        var product = await _context.Products.FindAsync(new object[] { productId }, cancellationToken);
        return product != null && product.InStock >= quantity;
    }

    #endregion

    #region Search Operations

    public async Task<PagedList<Product>> SearchProductsAsync(string searchTerm, ProductParams productParams, CancellationToken cancellationToken = default)
    {
        var query = _context.Products
            .Include(p => p.Photos)
            .Include(p => p.Categories)
                .ThenInclude(pc => pc.Category)
            .Include(p => p.Sizes) // Add this line to include sizes
            .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
            .AsQueryable();

        // Apply filtering
        if (productParams.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= productParams.MinPrice.Value);
        }

        if (productParams.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= productParams.MaxPrice.Value);
        }

        // Apply sorting
        query = productParams.OrderBy?.ToLower() switch
        {
            "name" => productParams.SortBy?.ToLower() == "desc"
                ? query.OrderByDescending(p => p.Name)
                : query.OrderBy(p => p.Name),
            "price" => productParams.SortBy?.ToLower() == "desc"
                ? query.OrderByDescending(p => p.Price)
                : query.OrderBy(p => p.Price),
            "created" => productParams.SortBy?.ToLower() == "desc"
                ? query.OrderByDescending(p => p.CreatedAt)
                : query.OrderBy(p => p.CreatedAt),
            _ => productParams.SortBy?.ToLower() == "desc"
                ? query.OrderByDescending(p => p.Name)
                : query.OrderBy(p => p.Name),
        };

        return await PagedList<Product>.CreateAsync(
            query,
            productParams.PageNumber,
            productParams.PageSize,
            cancellationToken
        );
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await _context.ProductCategories
            .Where(pc => pc.CategoryId == categoryId)
            .Select(pc => pc.Product)
            .Include(p => p.Photos)
            .Include(p => p.Sizes) // Add this line to include sizes
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetFeaturedProductsAsync(int count, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Include(p => p.Photos)
            .Include(p => p.Sizes) // Add this line to include sizes
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetNewArrivalsAsync(int count, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Include(p => p.Photos)
            .Include(p => p.Sizes) // Add this line to include sizes
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetRelatedProductsAsync(Guid productId, int count, CancellationToken cancellationToken = default)
    {
        // Get the categories of the specified product
        var productCategories = await _context.ProductCategories
            .Where(pc => pc.ProductId == productId)
            .Select(pc => pc.CategoryId)
            .ToListAsync(cancellationToken);

        // Get products that share categories with the specified product
        return await _context.Products
            .Include(p => p.Photos)
            .Include(p => p.Sizes) // Add this line to include sizes
            .Where(p => p.Id != productId && p.Categories.Any(pc => productCategories.Contains(pc.CategoryId)))
            .OrderByDescending(p => p.Categories.Count(pc => productCategories.Contains(pc.CategoryId))) // Order by number of matching categories
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    #endregion

    #region Size Operations

    public async Task<IEnumerable<ProductSize>> GetProductSizesAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _context.ProductSizes
            .Where(ps => ps.ProductId == productId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddSizeAsync(Guid productId, ProductSize size, CancellationToken cancellationToken = default)
    {
        // Set the ProductId
        size.ProductId = productId;

        _context.ProductSizes.Add(size);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateSizeAsync(Guid sizeId, string sizeName, int quantity, CancellationToken cancellationToken = default)
    {
        var size = await _context.ProductSizes.FindAsync(new object[] { sizeId }, cancellationToken);

        if (size != null)
        {
            size.Size = sizeName;
            size.Quantity = quantity;
            size.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task DeleteSizeAsync(Guid sizeId, CancellationToken cancellationToken = default)
    {
        var size = await _context.ProductSizes.FindAsync(new object[] { sizeId }, cancellationToken);

        if (size != null)
        {
            _context.ProductSizes.Remove(size);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products.AnyAsync(cancellationToken);
    }

    #endregion
}
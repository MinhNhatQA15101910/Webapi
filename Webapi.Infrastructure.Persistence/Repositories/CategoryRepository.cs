using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.Helpers;
using Webapi.SharedKernel.Params;

namespace Webapi.Infrastructure.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    #region Basic CRUD Operations

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<Category?> GetCategoryWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .Include(c => c.Products)
                .ThenInclude(pc => pc.Product)
                    .ThenInclude(p => p.Photos)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .ToListAsync(cancellationToken);
    }

    public async Task<PagedList<Category>> GetCategoriesAsync(CategoryParams categoryParams, CancellationToken cancellationToken = default)
    {
        var query = _context.Categories.AsQueryable();

        // Apply filtering
        if (!string.IsNullOrEmpty(categoryParams.Name))
        {
            query = query.Where(c => c.Name.Contains(categoryParams.Name));
        }

        if (!string.IsNullOrEmpty(categoryParams.Type))
        {
            query = query.Where(c => c.Type.Contains(categoryParams.Type));
        }

        // Apply sorting
        query = categoryParams.OrderBy?.ToLower() switch
        {
            "name" => categoryParams.SortBy?.ToLower() == "desc" 
                ? query.OrderByDescending(c => c.Name) 
                : query.OrderBy(c => c.Name),
            "created" => categoryParams.SortBy?.ToLower() == "desc" 
                ? query.OrderByDescending(c => c.CreatedAt) 
                : query.OrderBy(c => c.CreatedAt),
            _ => categoryParams.SortBy?.ToLower() == "desc" 
                ? query.OrderByDescending(c => c.Name) 
                : query.OrderBy(c => c.Name),
        };

        return await PagedList<Category>.CreateAsync(
            query,
            categoryParams.PageNumber,
            categoryParams.PageSize,
            cancellationToken
        );
    }

    public async Task<IEnumerable<Category>> FindAsync(Expression<Func<Category, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public void Add(Category category)
    {
        _context.Categories.Add(category);
    }

    public void Update(Category category)
    {
        category.UpdatedAt = DateTime.UtcNow;
        _context.Categories.Update(category);
    }

    public void Delete(Category category)
    {
        _context.Categories.Remove(category);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Categories.AnyAsync(c => c.Id == id, cancellationToken);
    }

    #endregion

    #region Product Operations

    public async Task<bool> HasProductsAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await _context.ProductCategories.AnyAsync(pc => pc.CategoryId == categoryId, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetCategoryProductsAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await _context.ProductCategories
            .Where(pc => pc.CategoryId == categoryId)
            .Select(pc => pc.Product)
            .Include(p => p.Photos)
            .ToListAsync(cancellationToken);
    }

    #endregion
}
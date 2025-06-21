using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.Helpers;
using Webapi.SharedKernel.Params;

namespace Webapi.Infrastructure.Persistence.Repositories;

public class ReviewRepository(AppDbContext context) : IReviewRepository
{
    #region Basic CRUD Operations

    public async Task<Review?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Reviews
            .FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<Review?> GetReviewWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Reviews
            .Include(r => r.Product)
            .Include(r => r.Owner)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Review>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Reviews
            .Include(r => r.Product)
            .Include(r => r.Owner)
            .ToListAsync(cancellationToken);
    }

    public async Task<PagedList<Review>> GetReviewsAsync(ReviewParams reviewParams, CancellationToken cancellationToken = default)
    {
        var query = context.Reviews
            .Include(r => r.Product)
            .Include(r => r.Owner)
            .AsQueryable();

        // Apply filtering
        if (!string.IsNullOrEmpty(reviewParams.Title))
        {
            query = query.Where(r => r.Title.Contains(reviewParams.Title));
        }

        if (reviewParams.ProductId.HasValue)
        {
            query = query.Where(r => r.ProductId == reviewParams.ProductId.Value);
        }

        if (reviewParams.OwnerId.HasValue)
        {
            query = query.Where(r => r.OwnerId == reviewParams.OwnerId.Value);
        }
        
        // Filter by owner email
        if (!string.IsNullOrEmpty(reviewParams.OwnerEmail))
        {
            query = query.Where(r => r.Owner.Email.Contains(reviewParams.OwnerEmail));
        }

        if (reviewParams.MinRating.HasValue)
        {
            query = query.Where(r => r.Rating >= reviewParams.MinRating.Value);
        }

        if (reviewParams.MaxRating.HasValue)
        {
            query = query.Where(r => r.Rating <= reviewParams.MaxRating.Value);
        }

        // Apply sorting
        query = reviewParams.OrderBy?.ToLower() switch
        {
            "title" => reviewParams.SortBy?.ToLower() == "desc"
                ? query.OrderByDescending(r => r.Title)
                : query.OrderBy(r => r.Title),
            "rating" => reviewParams.SortBy?.ToLower() == "desc"
                ? query.OrderByDescending(r => r.Rating)
                : query.OrderBy(r => r.Rating),
            "created" => reviewParams.SortBy?.ToLower() == "desc"
                ? query.OrderByDescending(r => r.CreatedAt)
                : query.OrderBy(r => r.CreatedAt),
            _ => reviewParams.SortBy?.ToLower() == "desc"
                ? query.OrderByDescending(r => r.CreatedAt)
                : query.OrderBy(r => r.CreatedAt),
        };

        return await PagedList<Review>.CreateAsync(
            query,
            reviewParams.PageNumber,
            reviewParams.PageSize,
            cancellationToken
        );
    }

    public async Task<IEnumerable<Review>> FindAsync(Expression<Func<Review, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await context.Reviews
            .Include(r => r.Product)
            .Include(r => r.Owner)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public void Add(Review review)
    {
        context.Reviews.Add(review);
    }

    public void Update(Review review)
    {
        review.UpdatedAt = DateTime.UtcNow;
        context.Reviews.Update(review);
    }

    public void Delete(Review review)
    {
        context.Reviews.Remove(review);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Reviews.AnyAsync(r => r.Id == id, cancellationToken);
    }

    #endregion

    #region Product Operations

    public async Task<IEnumerable<Review>> GetReviewsByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await context.Reviews
            .Where(r => r.ProductId == productId)
            .Include(r => r.Owner)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Review>> GetReviewsByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        return await context.Reviews
            .Where(r => r.OwnerId == ownerId)
            .Include(r => r.Product)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsOwnerAsync(Guid reviewId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await context.Reviews
            .AnyAsync(r => r.Id == reviewId && r.OwnerId == userId, cancellationToken);
    }

    public async Task<IEnumerable<Review>> GetReviewsByOwnerEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await context.Reviews
            .Where(r => r.Owner.Email == email)
            .Include(r => r.Product)
            .ToListAsync(cancellationToken);
    }

    #endregion
}
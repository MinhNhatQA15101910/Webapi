using System.Linq.Expressions;
using Webapi.Domain.Entities;
using Webapi.SharedKernel.Helpers;
using Webapi.SharedKernel.Params;

namespace Webapi.Domain.Interfaces;

public interface IReviewRepository
{
    // Basic CRUD operations
    Task<Review?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Review?> GetReviewWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Review>> GetAllAsync(CancellationToken cancellationToken = default);

    // Filtered queries
    Task<PagedList<Review>> GetReviewsAsync(ReviewParams reviewParams, CancellationToken cancellationToken = default);
    Task<IEnumerable<Review>> FindAsync(Expression<Func<Review, bool>> predicate, CancellationToken cancellationToken = default);
    Task<IEnumerable<Review>> GetReviewsByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Review>> GetReviewsByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Review>> GetReviewsByOwnerEmailAsync(string email, CancellationToken cancellationToken = default);

    // Create/Update/Delete
    void Add(Review review);
    void Update(Review review);
    void Delete(Review review);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> IsOwnerAsync(Guid reviewId, Guid userId, CancellationToken cancellationToken = default);
}
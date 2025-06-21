using Webapi.Domain.Entities;

namespace Webapi.Domain.Interfaces;

public interface ICartItemRepository
{
    Task<CartItem?> GetCartItemAsync(Guid userId, Guid productSizeId, CancellationToken cancellationToken = default);
    Task<CartItem?> GetCartItemByIdAsync(Guid cartItemId, CancellationToken cancellationToken = default);
    Task<CartItem?> GetCartItemWithDetailsAsync(Guid userId, Guid productSizeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CartItem>> GetCartItemsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CartItem>> GetCartItemsWithDetailsAsync(Guid userId, CancellationToken cancellationToken = default);
    void Add(CartItem cartItem);
    void Update(CartItem cartItem);
    void Remove(CartItem cartItem);
    Task ClearCartAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<int> GetCartItemCountAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<decimal> GetCartTotalAsync(Guid userId, CancellationToken cancellationToken = default);
}
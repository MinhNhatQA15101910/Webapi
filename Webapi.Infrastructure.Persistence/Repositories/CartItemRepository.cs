using Microsoft.EntityFrameworkCore;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;

namespace Webapi.Infrastructure.Persistence.Repositories;

public class CartItemRepository(AppDbContext context) : ICartItemRepository
{
    public async Task<CartItem?> GetCartItemAsync(Guid userId, Guid productSizeId, CancellationToken cancellationToken = default)
    {
        return await context.CartItems
            .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductSizeId == productSizeId, cancellationToken);
    }

    public async Task<CartItem?> GetCartItemWithDetailsAsync(Guid userId, Guid productSizeId, CancellationToken cancellationToken = default)
    {
        return await context.CartItems
            .Include(ci => ci.ProductSize)
            .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductSizeId == productSizeId, cancellationToken);
    }

    public async Task<IEnumerable<CartItem>> GetCartItemsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await context.CartItems
            .Where(ci => ci.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CartItem>> GetCartItemsWithDetailsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await context.CartItems
            .Where(ci => ci.UserId == userId)
            .Include(ci => ci.ProductSize)
                .ThenInclude(ps => ps.Product)
                    .ThenInclude(p => p.Photos)
            .ToListAsync(cancellationToken);
    }

    public void Add(CartItem cartItem)
    {
        context.CartItems.Add(cartItem);
    }

    public void Update(CartItem cartItem)
    {
        context.Entry(cartItem).State = EntityState.Modified;
    }

    public void Remove(CartItem cartItem)
    {
        context.CartItems.Remove(cartItem);
    }

    public async Task ClearCartAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var cartItems = await context.CartItems
            .Where(ci => ci.UserId == userId)
            .ToListAsync(cancellationToken);

        context.CartItems.RemoveRange(cartItems);
    }

    public async Task<int> GetCartItemCountAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await context.CartItems
            .Where(ci => ci.UserId == userId)
            .CountAsync(cancellationToken);
    }

    public async Task<decimal> GetCartTotalAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await context.CartItems
            .Where(ci => ci.UserId == userId)
            .SumAsync(ci => ci.ProductSize.Product.Price * ci.Quantity, cancellationToken);
    }

    public async Task<CartItem?> GetCartItemByIdAsync(Guid cartItemId, CancellationToken cancellationToken = default)
    {
        return await context.CartItems
            .Include(ci => ci.ProductSize)
                .ThenInclude(ps => ps.Product)
            .FirstOrDefaultAsync(ci => ci.Id == cartItemId, cancellationToken);
    }
}

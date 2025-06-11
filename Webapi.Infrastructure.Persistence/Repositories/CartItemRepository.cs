using Microsoft.EntityFrameworkCore;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.Infrastructure.Persistence.Data;

namespace Webapi.Infrastructure.Persistence.Repositories;

public class CartItemRepository(AppDbContext context) : ICartItemRepository
{
    private readonly AppDbContext _context = context;

    public async Task<CartItem?> GetCartItemAsync(Guid userId, Guid productId, CancellationToken cancellationToken = default)
    {
        return await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId, cancellationToken);
    }

    public async Task<CartItem?> GetCartItemWithDetailsAsync(Guid userId, Guid productId, CancellationToken cancellationToken = default)
    {
        return await _context.CartItems
            .Include(ci => ci.Product)
                .ThenInclude(p => p.Photos)
            .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId, cancellationToken);
    }

    public async Task<IEnumerable<CartItem>> GetCartItemsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.CartItems
            .Where(ci => ci.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CartItem>> GetCartItemsWithDetailsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.CartItems
            .Include(ci => ci.Product)
                .ThenInclude(p => p.Photos)
            .Where(ci => ci.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public void Add(CartItem cartItem)
    {
        _context.CartItems.Add(cartItem);
    }

    public void Update(CartItem cartItem)
    {
        _context.Entry(cartItem).State = EntityState.Modified;
    }

    public void Remove(CartItem cartItem)
    {
        _context.CartItems.Remove(cartItem);
    }

    public async Task ClearCartAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var cartItems = await _context.CartItems
            .Where(ci => ci.UserId == userId)
            .ToListAsync(cancellationToken);

        _context.CartItems.RemoveRange(cartItems);
    }

    public async Task<int> GetCartItemCountAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.CartItems
            .Where(ci => ci.UserId == userId)
            .CountAsync(cancellationToken);
    }

    public async Task<decimal> GetCartTotalAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.CartItems
            .Where(ci => ci.UserId == userId)
            .Join(
                _context.Products,
                ci => ci.ProductId,
                p => p.Id,
                (ci, p) => new { Quantity = ci.Quantity, Price = p.Price }
            )
            .SumAsync(x => x.Quantity * x.Price, cancellationToken);
    }
}
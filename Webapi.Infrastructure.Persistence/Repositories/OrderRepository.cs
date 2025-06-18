using Microsoft.EntityFrameworkCore;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;

namespace Webapi.Infrastructure.Persistence.Repositories;

public class OrderRepository(AppDbContext dbContext) : IOrderRepository
{
    public void Add(Order order)
    {
        dbContext.Orders.Add(order);
    }

    public async Task<Order?> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken)
    {
        return await dbContext.Orders
            .Include(o => o.Products)
                .ThenInclude(op => op.ProductSize)
                    .ThenInclude(ps => ps.Product)
                        .ThenInclude(p => p.Photos)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
    }
}

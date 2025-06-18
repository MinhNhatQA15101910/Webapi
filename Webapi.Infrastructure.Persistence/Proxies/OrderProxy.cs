using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.Infrastructure.Persistence.Repositories;

namespace Webapi.Infrastructure.Persistence.Proxies;

public class OrderProxy(
    OrderRepository orderRepository,
    ICacheService cacheService
) : IOrderRepository
{
    public void Add(Order order)
    {
        orderRepository.Add(order);

        UpdateCacheForOrderAdded();
    }

    public async Task<Order?> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var cacheKey = $"Orders_/{orderId}";

        if (!cacheService.TryGetValue(cacheKey, out Order? order))
        {
            // Fetch countries from the database.
            order = await orderRepository.GetOrderByIdAsync(orderId, cancellationToken);

            cacheService.Set(cacheKey, order);
        }

        return order;
    }

    private void UpdateCacheForOrderAdded()
    {
        var cacheKey = "Orders_?";
        cacheService.RemoveKeysStartingWith(cacheKey);
    }
}

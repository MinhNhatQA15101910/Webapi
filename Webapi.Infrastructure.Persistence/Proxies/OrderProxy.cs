using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.Infrastructure.Persistence.Repositories;
using Webapi.SharedKernel.DTOs.Orders;
using Webapi.SharedKernel.Helpers;
using Webapi.SharedKernel.Params;

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
        return await orderRepository.GetOrderByIdAsync(orderId, cancellationToken);
        
        // var cacheKey = $"Orders_/{orderId}";

        // if (!cacheService.TryGetValue(cacheKey, out Order? order))
        // {
        //     // Fetch countries from the database.
        //     order = await orderRepository.GetOrderByIdAsync(orderId, cancellationToken);

        //     cacheService.Set(cacheKey, order);
        // }

        // return order;
    }

    public async Task<PagedList<OrderDto>> GetOrdersAsync(Guid? userId, OrderParams orderParams, CancellationToken cancellationToken = default)
    {
        return await orderRepository.GetOrdersAsync(userId, orderParams, cancellationToken);
    }

    public void Update(Order order)
    {
        orderRepository.Update(order);
    }

    private void UpdateCacheForOrderAdded()
    {
        var cacheKey = "Orders_?";
        cacheService.RemoveKeysStartingWith(cacheKey);
    }
}

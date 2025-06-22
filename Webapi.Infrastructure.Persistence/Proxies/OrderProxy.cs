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
        var cacheKey = $"Orders_/{orderId}";

        if (!cacheService.TryGetValue(cacheKey, out Order? order))
        {
            // Fetch countries from the database.
            order = await orderRepository.GetOrderByIdAsync(orderId, cancellationToken);

            cacheService.Set(cacheKey, order);
        }

        return order;
    }

    public async Task<PagedList<OrderDto>> GetOrdersAsync(Guid? userId, OrderParams orderParams, CancellationToken cancellationToken = default)
    {
        var cacheKey = GetOrdersQueryCacheKey(userId, orderParams);

        if (!cacheService.TryGetValue(cacheKey, out PagedList<OrderDto>? orders))
        {
            // Fetch countries from the database.
            orders = await orderRepository.GetOrdersAsync(userId, orderParams, cancellationToken);

            cacheService.Set(cacheKey, orders);
        }

        return orders ?? new PagedList<OrderDto>([], 0, orderParams.PageNumber, orderParams.PageSize);
    }

    public void Update(Order order)
    {
        orderRepository.Update(order);

        UpdateCacheForOrderUpdated(order);
    }

    private static string GetOrdersQueryCacheKey(Guid? currentUserId, OrderParams orderParams)
    {
        var cacheKey = $"Orders_";
        cacheKey += $"?pageNumber={orderParams.PageNumber}";
        cacheKey += $"&pageSize={orderParams.PageSize}";
        if (currentUserId.HasValue)
            cacheKey += $"&currentUserId={currentUserId.Value}";
        if (orderParams.ShippingType is not null)
            cacheKey += $"&shippingType={orderParams.ShippingType}";
        if (orderParams.OrderState is not null)
            cacheKey += $"&orderState={orderParams.OrderState}";
        cacheKey += $"&minPrice={orderParams.MinPrice}";
        cacheKey += $"&maxPrice={orderParams.MaxPrice}";
        cacheKey += $"&orderBy={orderParams.OrderBy}";
        cacheKey += $"&sortBy={orderParams.SortBy}";
        return cacheKey;
    }

    private void UpdateCacheForOrderAdded()
    {
        var cacheKey = "Orders_?";
        cacheService.RemoveKeysStartingWith(cacheKey);
    }

    private void UpdateCacheForOrderUpdated(Order order)
    {
        var cacheKeyForList = "Orders_?";
        cacheService.RemoveKeysStartingWith(cacheKeyForList);

        var cacheKeyForSingle = $"Orders_/{order.Id}";
        cacheService.Set(cacheKeyForSingle, order);
    }
}

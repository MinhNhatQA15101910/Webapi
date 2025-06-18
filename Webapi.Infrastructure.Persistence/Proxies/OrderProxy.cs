using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.Infrastructure.Persistence.Repositories;

namespace Webapi.Infrastructure.Persistence.Proxies;

public class OrderProxy(OrderRepository orderRepository) : IOrderRepository
{
    public void Add(Order order)
    {
        orderRepository.Add(order);
    }

    public async Task<Order?> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken)
    {
        return await orderRepository.GetOrderByIdAsync(orderId, cancellationToken);
    }
}

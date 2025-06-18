using Webapi.Domain.Entities;

namespace Webapi.Domain.Interfaces;

public interface IOrderRepository
{
    void Add(Order order);
    Task<Order?> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken);
}

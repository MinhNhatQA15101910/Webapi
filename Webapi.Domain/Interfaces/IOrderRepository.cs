using Webapi.Domain.Entities;
using Webapi.SharedKernel.DTOs.Orders;
using Webapi.SharedKernel.Helpers;
using Webapi.SharedKernel.Params;

namespace Webapi.Domain.Interfaces;

public interface IOrderRepository
{
    void Add(Order order);
    Task<Order?> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken);
    Task<PagedList<OrderDto>> GetOrdersAsync(Guid? userId, OrderParams orderParams, CancellationToken cancellationToken = default);
    void Update(Order order);
}

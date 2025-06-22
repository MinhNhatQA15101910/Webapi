using Webapi.Domain.Entities;

namespace Webapi.Application.OrdersCQRS.Observers.OrderCreated;

public interface IOrderCreatedListener
{
    Task UpdateAsync(Order order, List<CartItem> cartItems, List<Voucher> vouchers, CancellationToken cancellationToken = default);
}

using Webapi.Domain.Entities;

namespace Webapi.Application.OrdersCQRS.Observers;

public interface IOrderCreatedListener
{
    Task UpdateAsync(List<CartItem> cartItems, CancellationToken cancellationToken = default);
}

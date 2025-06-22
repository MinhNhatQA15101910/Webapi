using Webapi.Application.Common.Exceptions;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;

namespace Webapi.Application.OrdersCQRS.Observers.OrderCreated;

public class UpdateCartOrderCreatedListener(IUnitOfWork unitOfWork) : IOrderCreatedListener
{
    public async Task UpdateAsync(Order order, List<CartItem> cartItems, List<Voucher> vouchers, CancellationToken cancellationToken = default)
    {
        foreach (var cartItem in cartItems)
        {
            unitOfWork.CartItemRepository.Remove(cartItem);
        }

        if (!await unitOfWork.CompleteAsync(cancellationToken))
        {
            throw new BadRequestException("Failed to update cart items after order creation.");
        }
    }
}

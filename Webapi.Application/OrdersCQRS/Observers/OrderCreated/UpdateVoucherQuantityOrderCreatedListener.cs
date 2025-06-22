using Webapi.Application.Common.Exceptions;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;

namespace Webapi.Application.OrdersCQRS.Observers.OrderCreated;

public class UpdateVoucherQuantityOrderCreatedListener(IUnitOfWork unitOfWork) : IOrderCreatedListener
{
    public async Task UpdateAsync(Order order, List<CartItem> cartItems, List<Voucher> vouchers, CancellationToken cancellationToken = default)
    {
        foreach (var voucher in vouchers)
        {
            voucher.Quantity -= 1;
            voucher.UpdatedAt = DateTime.UtcNow;
        }

        if (!await unitOfWork.CompleteAsync(cancellationToken))
        {
            throw new BadRequestException("Failed to update voucher quantities after order creation.");
        }
    }
}

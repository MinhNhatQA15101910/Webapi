
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Exceptions.ProductSize;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;

namespace Webapi.Application.OrdersCQRS.Observers;

public class UpdateProductQuantityOrderCreatedListener(IUnitOfWork unitOfWork) : IOrderCreatedListener
{
    public async Task UpdateAsync(List<CartItem> cartItems, CancellationToken cancellationToken = default)
    {
        foreach (var cartItem in cartItems)
        {
            var productSize = await unitOfWork.ProductSizeRepository.GetByIdAsync(cartItem.ProductSizeId, cancellationToken)
                ?? throw new ProductSizeNotFoundException(cartItem.ProductSizeId);

            productSize.Quantity -= cartItem.Quantity;
            productSize.UpdatedAt = DateTime.UtcNow;
        }

        if (!await unitOfWork.CompleteAsync(cancellationToken))
        {
            throw new BadRequestException("Failed to update product quantities after order creation.");
        }
    }
}

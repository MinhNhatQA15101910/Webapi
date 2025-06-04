namespace Webapi.Application.Common.Exceptions.CartItem;

public class CartItemUpdateException : BadRequestException
{
    public CartItemUpdateException(Guid productId, string message)
        : base($"Failed to update cart item for product {productId}: {message}")
    {
    }

    public CartItemUpdateException(Guid productId)
        : base($"An error occurred while updating cart item for product {productId}.")
    {
    }
}
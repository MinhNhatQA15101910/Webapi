namespace Webapi.Application.Common.Exceptions.CartItem;

public class CartItemUpdateException : BadRequestException
{
    public CartItemUpdateException(Guid cartItemId, string message)
        : base($"Failed to update cart item with ID {cartItemId}: {message}")
    {
    }

    public CartItemUpdateException(Guid cartItemId)
        : base($"An error occurred while updating cart item with ID {cartItemId}.")
    {
    }
}
namespace Webapi.Application.Common.Exceptions.CartItem;

public class CartItemDeleteException : BadRequestException
{
    public CartItemDeleteException(Guid cartItemId, string message)
        : base($"Failed to delete cart item with ID {cartItemId}: {message}")
    {
    }

    public CartItemDeleteException(Guid cartItemId)
        : base($"An error occurred while deleting cart item with ID {cartItemId}.")
    {
    }
}
namespace Webapi.Application.Common.Exceptions.CartItem;

public class CartItemDeleteException : BadRequestException
{
    public CartItemDeleteException(Guid productId, string message)
        : base($"Failed to delete cart item for product {productId}: {message}")
    {
    }

    public CartItemDeleteException(Guid productId)
        : base($"An error occurred while deleting cart item for product {productId}.")
    {
    }
}
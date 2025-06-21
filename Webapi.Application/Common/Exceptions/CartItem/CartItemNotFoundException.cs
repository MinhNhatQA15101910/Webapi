namespace Webapi.Application.Common.Exceptions.CartItem;

public class CartItemNotFoundException : NotFoundException
{
    public CartItemNotFoundException(Guid userId, Guid productId)
        : base($"The cart item for user {userId} and product {productId} was not found.")
    {
    }

    public CartItemNotFoundException(Guid cartItemId)
        : base($"The cart item with ID {cartItemId} was not found.")
    {
    }
}
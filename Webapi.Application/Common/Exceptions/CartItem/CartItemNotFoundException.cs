namespace Webapi.Application.Common.Exceptions.CartItem;

public class CartItemNotFoundException : NotFoundException
{
    public CartItemNotFoundException(Guid userId, Guid productId)
        : base($"The cart item for user {userId} and product {productId} was not found.")
    {
    }
    
    public CartItemNotFoundException(string message)
        : base(message)
    {
    }
}
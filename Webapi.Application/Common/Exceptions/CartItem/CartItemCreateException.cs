namespace Webapi.Application.Common.Exceptions.CartItem;

public class CartItemCreateException : BadRequestException
{
    public CartItemCreateException(string message)
        : base($"Failed to create cart item: {message}")
    {
    }

    public CartItemCreateException()
        : base("An error occurred while creating the cart item.")
    {
    }
}
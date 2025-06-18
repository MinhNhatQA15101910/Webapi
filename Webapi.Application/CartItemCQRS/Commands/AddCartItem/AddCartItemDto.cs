namespace Webapi.Application.CartItemCQRS.Commands.AddCartItem;

public class AddCartItemDto
{
    public Guid ProductSizeId { get; set; }
    public int Quantity { get; set; }
}

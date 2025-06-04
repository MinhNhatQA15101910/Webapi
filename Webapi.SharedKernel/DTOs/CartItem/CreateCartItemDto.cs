namespace Webapi.SharedKernel.DTOs.CartItem;

public class CreateCartItemDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; } = 1;
}
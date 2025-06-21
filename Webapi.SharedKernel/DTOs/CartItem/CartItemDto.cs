namespace Webapi.SharedKernel.DTOs.CartItem;

public class CartItemDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ProductSizeId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal ProductPrice { get; set; }
    public string? ProductPhotoUrl { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

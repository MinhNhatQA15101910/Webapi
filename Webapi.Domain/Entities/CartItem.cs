namespace Webapi.Domain.Entities;

public class CartItem
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid ProductSizeId { get; set; }
    public ProductSize ProductSize { get; set; } = null!;
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

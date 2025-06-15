namespace Webapi.Domain.Entities;

public class ProductSize
{
    public Guid Id { get; set; }
    public string Size { get; set; } = string.Empty;
    public int Quantity { get; set; }

    // Make ProductId nullable
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<OrderProduct> Orders { get; set; } = [];
    public ICollection<CartItem> CartItems { get; set; } = [];
}

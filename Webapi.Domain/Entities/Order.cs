namespace Webapi.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public double TotalPrice { get; set; }
    public string ShippingType { get; set; } = string.Empty;
    public double ShippingCost { get; set; }
    public string OrderStatus { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<OrderProduct> Products { get; set; } = [];
    public ICollection<OrderVoucher> Vouchers { get; set; } = [];

    // Navigation properties
    public Guid OwnerId { get; set; }
    public User Owner { get; set; } = null!;
}

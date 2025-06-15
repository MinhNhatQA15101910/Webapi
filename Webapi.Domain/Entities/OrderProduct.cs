using System.ComponentModel.DataAnnotations.Schema;

namespace Webapi.Domain.Entities;

[Table("OrderProducts")]
public class OrderProduct
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public Guid ProductSizeId { get; set; }
    public ProductSize ProductSize { get; set; } = null!;
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

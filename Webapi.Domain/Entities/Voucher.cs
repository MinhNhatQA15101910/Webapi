namespace Webapi.Domain.Entities;

public class Voucher
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Value { get; set; }
    public DateTime ExpiredAt { get; set; } = DateTime.UtcNow;
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<OrderVoucher> Orders { get; set; } = [];
}

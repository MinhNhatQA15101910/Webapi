namespace Webapi.Domain.Entities;

public class Voucher
{
    public Guid Id { get; set; }

    public int Quantity { get; set; }
    public VoucherType Type { get; set; } = null!;
    public Guid TypeId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiredAt { get; set; } = DateTime.UtcNow.AddMonths(3);
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<OrderVoucher> Orders { get; set; } = [];
    public ICollection<VoucherItem> Items { get; set; } = [];
}
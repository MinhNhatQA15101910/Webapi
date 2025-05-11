using System.ComponentModel.DataAnnotations.Schema;

namespace Webapi.Domain.Entities;

[Table("OrderVouchers")]
public class OrderVoucher
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public Guid VoucherId { get; set; }
    public Voucher Voucher { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

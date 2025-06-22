namespace Webapi.SharedKernel.DTOs.Voucher;

public class VoucherDto
{
    public Guid Id { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public double Value { get; set; }
    public int Quantity { get; set; }
    public int AvailableQuantity { get; set; } // Number of items still available
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime ExpiredAt { get; set; }
    public bool IsExpired => DateTime.UtcNow > ExpiredAt;
}

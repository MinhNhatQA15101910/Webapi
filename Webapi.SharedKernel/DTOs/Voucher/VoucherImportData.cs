namespace Webapi.SharedKernel.DTOs.Voucher;

public class VoucherImportDto
{
    public string Name { get; set; } = string.Empty;
    public double Value { get; set; }
    public int Quantity { get; set; }
    public DateTime? ExpiredAt { get; set; }
}
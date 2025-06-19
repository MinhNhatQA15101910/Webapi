namespace Webapi.SharedKernel.DTOs.Voucher;

public class CreateVoucherDto
{
    public string Name { get; set; } = string.Empty;
    public double Value { get; set; }
    public DateTime? ExpiredAt { get; set; }
    public int Quantity { get; set; }
}
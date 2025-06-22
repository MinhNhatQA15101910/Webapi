namespace Webapi.SharedKernel.DTOs.Voucher;

public class VoucherItemDto
{
    public Guid Id { get; set; }
    public string VoucherId { get; set; } = string.Empty;
    public bool Status { get; set; }
}
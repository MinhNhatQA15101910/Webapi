namespace Webapi.SharedKernel.DTOs.Orders;

public class OrderVoucherDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Value { get; set; }
}

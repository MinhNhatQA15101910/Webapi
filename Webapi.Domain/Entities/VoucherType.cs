namespace Webapi.Domain.Entities;

public class VoucherType
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Value { get; set; }
}
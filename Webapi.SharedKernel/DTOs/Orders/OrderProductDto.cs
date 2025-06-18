namespace Webapi.SharedKernel.DTOs.Orders;

public class OrderProductDto
{
    public Guid ProductSizeId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal ProductPrice { get; set; }
    public int Quantity { get; set; }
    public string? ProductPhotoUrl { get; set; }
}

namespace Webapi.SharedKernel.DTOs.Orders;

public class OrderDto
{
    public Guid Id { get; set; }
    public double TotalPrice { get; set; }
    public string ShippingType { get; set; } = string.Empty;
    public double ShippingCost { get; set; }
    public string OrderState { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<OrderProductDto> Products { get; set; } = [];
}

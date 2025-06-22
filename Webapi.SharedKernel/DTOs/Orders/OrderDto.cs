namespace Webapi.SharedKernel.DTOs.Orders;

public class OrderDto
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public decimal TotalPrice { get; set; }
    public string ShippingType { get; set; } = string.Empty;
    public double ShippingCost { get; set; }
    public string OrderState { get; set; } = string.Empty;
    public AddressDto Address { get; set; } = new AddressDto();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<OrderProductDto> Products { get; set; } = [];
    public List<OrderVoucherDto> Vouchers { get; set; } = [];
}

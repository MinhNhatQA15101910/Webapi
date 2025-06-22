namespace Webapi.Application.OrdersCQRS.Commands.CreateOrder;

public class CreateOrderDto
{
    public List<Guid> CartItemIds { get; set; } = [];
    public List<Guid> VoucherIds { get; set; } = [];
    public string ShippingType { get; set; } = string.Empty;
    public string ReceiverName { get; set; } = string.Empty;
    public string ReceiverEmail { get; set; } = string.Empty;
    public string DetailAddress { get; set; } = string.Empty;
}

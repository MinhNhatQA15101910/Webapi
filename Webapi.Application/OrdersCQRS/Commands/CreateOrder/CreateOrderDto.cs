namespace Webapi.Application.OrdersCQRS.Commands.CreateOrder;

public class CreateOrderDto
{
    public List<Guid> CartItemIds { get; set; } = [];
    public string ShippingType { get; set; } = string.Empty;
}

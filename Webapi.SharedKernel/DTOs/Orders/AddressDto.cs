namespace Webapi.SharedKernel.DTOs.Orders;

public class AddressDto
{
    public string ReceiverName { get; set; } = string.Empty;
    public string ReceiverEmail { get; set; } = string.Empty;
    public string DetailAddress { get; set; } = string.Empty;
}

namespace Webapi.Domain.ValueObjects;

public class Address
{
    public string ReceiverName { get; set; } = string.Empty;
    public string ReceiverEmail { get; set; } = string.Empty;
    public string DetailAddress { get; set; } = string.Empty;
}

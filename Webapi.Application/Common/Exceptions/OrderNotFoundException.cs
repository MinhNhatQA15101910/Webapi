namespace Webapi.Application.Common.Exceptions;

public class OrderNotFoundException(Guid orderId)
    : NotFoundException($"The order with the identifier {orderId} was not found.")
{
}

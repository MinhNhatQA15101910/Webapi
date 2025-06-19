using Webapi.Domain.Interfaces.States;

namespace Webapi.Domain.Enums;

public enum OrderStates
{
    None = 0,
    Pending = 1,
    Packaged = 2,
    InDelivery = 3,
    Undelivered = 4,
    Completed = 5,
    Cancelled = 6,
}

public static class OrderStateFactory
{
    public static IOrderState Create(OrderStates status)
    {
        return status switch
        {
            OrderStates.Pending => new PendingState(),
            OrderStates.Packaged => new PackagedState(),
            OrderStates.InDelivery => new InDeliveryState(),
            OrderStates.Undelivered => new UndeliveredState(),
            OrderStates.Completed => new CompletedState(),
            OrderStates.Cancelled => new CancelledState(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static IOrderState Create(string status)
    {
        if (Enum.TryParse<OrderStates>(status, out var orderState))
        {
            return Create(orderState);
        }
        throw new ArgumentException($"Invalid order state: {status}");
    }
}

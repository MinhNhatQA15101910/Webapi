using Webapi.Domain.Entities;
using Webapi.Domain.Enums;

namespace Webapi.Domain.Interfaces.States;

public class PackagedState : IOrderState
{
    public void Next(Order order)
    {
        order.SetState(new InDeliveryState());
    }

    public void Cancel(Order order)
    {
        order.SetState(new CancelledState());
    }

    public OrderStates GetStatus()
    {
        return OrderStates.Packaged;
    }
}

using Webapi.Domain.Entities;
using Webapi.Domain.Enums;

namespace Webapi.Domain.Interfaces.States;

public class InDeliveryState : IOrderState
{
    public void Next(Order order)
    {
        order.SetState(new CompletedState());
    }

    public void Cancel(Order order)
    {
        order.SetState(new UndeliveredState());
    }

    public OrderStates GetStatus()
    {
        return OrderStates.InDelivery;
    }
}

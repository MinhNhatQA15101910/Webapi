using Webapi.Domain.Entities;
using Webapi.Domain.Enums;

namespace Webapi.Domain.Interfaces.States;

public class InDeliveryState : IOrderState
{
    public void Next(OrderContext orderContext)
    {
        orderContext.SetState(new CompletedState());
    }

    public void Cancel(OrderContext orderContext)
    {
        orderContext.SetState(new UndeliveredState());
    }

    public OrderStates GetStatus()
    {
        return OrderStates.InDelivery;
    }
}

using Webapi.Domain.Entities;
using Webapi.Domain.Enums;

namespace Webapi.Domain.Interfaces.States;

public class PackagedState : IOrderState
{
    public void Next(OrderContext orderContext)
    {
        orderContext.SetState(new InDeliveryState());
    }

    public void Cancel(OrderContext orderContext)
    {
        orderContext.SetState(new CancelledState());
    }

    public OrderStates GetStatus()
    {
        return OrderStates.Packaged;
    }
}

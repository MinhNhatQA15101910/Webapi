using Webapi.Domain.Entities;
using Webapi.Domain.Enums;

namespace Webapi.Domain.Interfaces.States;

public class PendingState : IOrderState
{
    public void Next(OrderContext orderContext)
    {
        orderContext.SetState(new PackagedState());
    }

    public void Cancel(OrderContext orderContext)
    {
        orderContext.SetState(new CancelledState());
    }

    public OrderStates GetStatus()
    {
        return OrderStates.Pending;
    }
}

using Webapi.Domain.Entities;
using Webapi.Domain.Enums;

namespace Webapi.Domain.Interfaces.States;

public class PackagedState(OrderContext orderContext) : IOrderState
{
    private readonly OrderContext _orderContext = orderContext;

    public void Next()
    {
        _orderContext.SetState(new InDeliveryState(_orderContext));
    }

    public void Cancel()
    {
        _orderContext.SetState(new CancelledState());
    }

    public OrderStates GetStatus()
    {
        return OrderStates.Packaged;
    }
}

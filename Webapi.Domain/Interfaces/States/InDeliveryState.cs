using Webapi.Domain.Entities;
using Webapi.Domain.Enums;

namespace Webapi.Domain.Interfaces.States;

public class InDeliveryState(OrderContext orderContext) : IOrderState
{
    private readonly OrderContext _orderContext = orderContext;

    public void Next()
    {
        _orderContext.SetState(new CompletedState());
    }

    public void Cancel()
    {
        _orderContext.SetState(new UndeliveredState());
    }

    public OrderStates GetStatus()
    {
        return OrderStates.InDelivery;
    }
}

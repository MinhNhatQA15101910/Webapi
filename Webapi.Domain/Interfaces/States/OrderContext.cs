using Webapi.Domain.Entities;
using Webapi.Domain.Enums;

namespace Webapi.Domain.Interfaces.States;

public class OrderContext
{
    private readonly Order _order;
    private IOrderState _state;

    public OrderContext(Order order)
    {
        _order = order;
        _state = OrderStateFactory.Create(order.OrderState, this);
    }

    public void SetState(IOrderState state)
    {
        _state = state;
        _order.OrderState = _state.GetStatus().ToString();
    }

    public void NextState()
    {
        _state.Next();
    }

    public void Cancel()
    {
        _state.Cancel();
    }
}

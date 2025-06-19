using Webapi.Domain.Entities;
using Webapi.Domain.Enums;

namespace Webapi.Domain.Interfaces.States;

public class OrderContext(Order order)
{
    private IOrderState _state = OrderStateFactory.Create(order.OrderState);

    public void SetState(IOrderState state)
    {
        _state = state;
        order.OrderState = _state.GetStatus().ToString();
    }

    public void NextState()
    {
        _state.Next(this);
    }

    public void Cancel()
    {
        _state.Cancel(this);
    }
}

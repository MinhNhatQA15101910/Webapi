using Webapi.Domain.Entities;
using Webapi.Domain.Enums;

namespace Webapi.Domain.Interfaces.States;

public class CancelledState : IOrderState
{
    public void Next(OrderContext orderContext)
    {
        Console.WriteLine("Order is cancelled and cannot proceed to the next state.");
    }

    public void Cancel(OrderContext orderContext)
    {
        Console.WriteLine("Order is already cancelled.");
    }

    public OrderStates GetStatus()
    {
        return OrderStates.Cancelled;
    }
}

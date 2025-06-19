using Webapi.Domain.Entities;
using Webapi.Domain.Enums;

namespace Webapi.Domain.Interfaces.States;

public class UndeliveredState : IOrderState
{
    public void Cancel(OrderContext orderContext)
    {
        Console.WriteLine("Cannot cancel an undelivered order.");
    }

    public OrderStates GetStatus()
    {
        return OrderStates.Undelivered;
    }

    public void Next(OrderContext orderContext)
    {
        Console.WriteLine("Order is undelivered and cannot proceed to the next state.");
    }
}

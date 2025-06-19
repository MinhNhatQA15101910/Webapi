using Webapi.Domain.Entities;
using Webapi.Domain.Enums;

namespace Webapi.Domain.Interfaces.States;

public class UndeliveredState : IOrderState
{
    public void Cancel(Order order)
    {
        Console.WriteLine("Cannot cancel an undelivered order.");
    }

    public OrderStates GetStatus()
    {
        return OrderStates.Undelivered;
    }

    public void Next(Order order)
    {
        Console.WriteLine("Order is undelivered and cannot proceed to the next state.");
    }
}

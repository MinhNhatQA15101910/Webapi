using Webapi.Domain.Enums;

namespace Webapi.Domain.Interfaces.States;

public class UndeliveredState : IOrderState
{
    public void Cancel()
    {
        Console.WriteLine("Cannot cancel an undelivered order.");
    }

    public OrderStates GetStatus()
    {
        return OrderStates.Undelivered;
    }

    public void Next()
    {
        Console.WriteLine("Order is undelivered and cannot proceed to the next state.");
    }
}

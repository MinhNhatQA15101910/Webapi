using Webapi.Domain.Entities;
using Webapi.Domain.Enums;

namespace Webapi.Domain.Interfaces.States;

public class CompletedState : IOrderState
{
    public void Cancel(OrderContext orderContext)
    {
        Console.WriteLine("Cannot cancel a completed order.");
    }

    public OrderStates GetStatus()
    {
        return OrderStates.Completed;
    }

    public void Next(OrderContext orderContext)
    {
        Console.WriteLine("Order is already completed. No next state available.");
    }
}

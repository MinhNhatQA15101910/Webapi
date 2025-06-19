using Webapi.Domain.Entities;
using Webapi.Domain.Enums;

namespace Webapi.Domain.Interfaces.States;

public interface IOrderState
{
    void Next(OrderContext orderContext);
    void Cancel(OrderContext orderContext);
    OrderStates GetStatus();
}

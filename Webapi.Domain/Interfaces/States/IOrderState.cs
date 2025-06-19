using Webapi.Domain.Entities;
using Webapi.Domain.Enums;

namespace Webapi.Domain.Interfaces.States;

public interface IOrderState
{
    void Next(Order order);
    void Cancel(Order order);
    OrderStates GetStatus();
}

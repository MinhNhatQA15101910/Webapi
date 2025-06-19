using Webapi.Domain.Enums;
using Webapi.Domain.Interfaces.States;

namespace Webapi.Domain.Entities;

public class Order
{
    #region Properties
    public Guid Id { get; set; }
    public decimal TotalPrice { get; set; }
    public string ShippingType { get; set; } = string.Empty;
    public double ShippingCost { get; set; }
    public string OrderState { get; set; } = OrderStates.Pending.ToString();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<OrderProduct> Products { get; set; } = [];
    public ICollection<OrderVoucher> Vouchers { get; set; } = [];

    // Navigation properties
    public Guid OwnerId { get; set; }
    public User Owner { get; set; } = null!;
    #endregion

    private IOrderState _state;

    public Order()
    {
        _state = OrderStateFactory.Create(OrderState);
    }

    public void SetState(IOrderState state)
    {
        _state = state;
        OrderState = _state.GetStatus().ToString();
    }

    public void NextState()
    {
        _state.Next(this);
    }

    public void Cancel()
    {
        _state.Cancel(this);
    }

    public OrderStates GetStatus()
    {
        return _state.GetStatus();
    }
}

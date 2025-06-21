namespace Webapi.Domain.Enums;

public enum OrderStates
{
    None = 0,
    Pending = 1,
    Packaged = 2,
    Shipped = 3,
    Undelivered = 4,
    Completed = 5,
    Cancelled = 6,
}

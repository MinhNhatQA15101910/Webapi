namespace Webapi.SharedKernel.Params;

public class OrderParams : PaginationParams
{
    public string? ShippingType { get; set; }
    public string? OrderState { get; set; }
    public decimal MinPrice { get; set; } = 0;
    public decimal MaxPrice { get; set; } = decimal.MaxValue;
    public string OrderBy { get; set; } = "createdAt";
    public string SortBy { get; set; } = "desc";
}

namespace Webapi.SharedKernel.Params;

public class ProductParams : PaginationParams
{
    public string? Name { get; set; }
    public string? Category { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string OrderBy { get; set; } = "name";
    public string SortBy { get; set; } = "asc";
}
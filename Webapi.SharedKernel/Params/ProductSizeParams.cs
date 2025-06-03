namespace Webapi.SharedKernel.Params;

public class ProductSizeParams : PaginationParams
{
    public string? Size { get; set; }
    public Guid? ProductId { get; set; }
    public string OrderBy { get; set; } = "size";
    public string SortBy { get; set; } = "asc";
}
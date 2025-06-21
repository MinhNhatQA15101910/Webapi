namespace Webapi.SharedKernel.Params;

public class CategoryParams : PaginationParams
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string OrderBy { get; set; } = "name";
    public string SortBy { get; set; } = "asc";
}
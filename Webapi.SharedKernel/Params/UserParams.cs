namespace Webapi.SharedKernel.Params;

public class UserParams : PaginationParams
{
    public string? Email { get; set; }
    public string OrderBy { get; set; } = "email";
    public string SortBy { get; set; } = "asc";
}


namespace Webapi.SharedKernel.Params;

public class ReviewParams : PaginationParams
{
    public string? Title { get; set; }
    public float? MinRating { get; set; }
    public float? MaxRating { get; set; }
    public Guid? ProductId { get; set; }
    public Guid? OwnerId { get; set; }
    public string? OwnerEmail { get; set; } 
    public string? OrderBy { get; set; } = "created";
    public string? SortBy { get; set; } = "desc";
}
namespace Webapi.SharedKernel.DTOs.Review;

public class CreateReviewDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public float Rating { get; set; }
    public Guid ProductId { get; set; }
}
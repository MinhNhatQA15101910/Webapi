namespace Webapi.SharedKernel.DTOs.Product;

public class ProductRatingDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public float AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public int FiveStarCount { get; set; }
    public int FourStarCount { get; set; } 
    public int ThreeStarCount { get; set; }
    public int TwoStarCount { get; set; }
    public int OneStarCount { get; set; }
}
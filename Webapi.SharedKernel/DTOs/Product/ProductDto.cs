using Webapi.SharedKernel.DTOs.ProductPhoto;

namespace Webapi.SharedKernel.DTOs.Product;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int InStock { get; set; }
    public string? MainPhotoUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<ProductPhotoDto> Photos { get; set; } = new List<ProductPhotoDto>();
    public ICollection<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
    public ICollection<ProductSizeDto> Sizes { get; set; } = new List<ProductSizeDto>();
}
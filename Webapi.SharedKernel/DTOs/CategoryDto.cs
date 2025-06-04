using Webapi.SharedKernel.DTOs.Product;

namespace Webapi.SharedKernel.DTOs;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<ProductDto> Products { get; set; } = new List<ProductDto>();
}

public class CreateCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}

public class UpdateCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}
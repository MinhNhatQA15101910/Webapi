namespace Webapi.SharedKernel.DTOs;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int InStock { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? MainPhotoUrl { get; set; }
    public ICollection<ProductPhotoDto> Photos { get; set; } = new List<ProductPhotoDto>();
    public ICollection<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
}

public class ProductPhotoDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? PublicId { get; set; }
    public bool IsMain { get; set; }
}

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int InStock { get; set; }
    public ICollection<Guid> CategoryIds { get; set; } = new List<Guid>();
}

public class UpdateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int InStock { get; set; }
    public ICollection<Guid> CategoryIds { get; set; } = new List<Guid>();
}
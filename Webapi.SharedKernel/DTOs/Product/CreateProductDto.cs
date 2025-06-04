using Microsoft.AspNetCore.Http;
using Webapi.SharedKernel.DTOs.ProductSize;

namespace Webapi.SharedKernel.DTOs.Product;

public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int InStock { get; set; }
    public ICollection<Guid> CategoryIds { get; set; } = new List<Guid>();
    public ICollection<Guid> SizeIds { get; set; } = new List<Guid>();
    public ICollection<CreateProductSizeDto> Sizes { get; set; } = new List<CreateProductSizeDto>();
    
    public IFormFile? MainImage { get; set; }
    public ICollection<IFormFile>? AdditionalImages { get; set; }
}
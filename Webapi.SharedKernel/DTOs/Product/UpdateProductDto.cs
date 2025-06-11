using Microsoft.AspNetCore.Http;
using Webapi.SharedKernel.DTOs.ProductSize;

namespace Webapi.SharedKernel.DTOs.Product;

public class UpdateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int InStock { get; set; }
    public ICollection<Guid> CategoryIds { get; set; } = new List<Guid>();
    
    // Photo management properties
    public IFormFile? MainImage { get; set; }
    public ICollection<IFormFile>? AdditionalImages { get; set; }
    public Guid? MainPhotoId { get; set; } 
    public ICollection<Guid>? PhotoIdsToRemove { get; set; }
    
    // Product size management properties
    public ICollection<CreateProductSizeDto>? SizesToAdd { get; set; } = new List<CreateProductSizeDto>();
    public ICollection<UpdateProductSizeDto>? SizesToUpdate { get; set; } = new List<UpdateProductSizeDto>();
    public ICollection<Guid>? SizeIdsToRemove { get; set; } = new List<Guid>();
}
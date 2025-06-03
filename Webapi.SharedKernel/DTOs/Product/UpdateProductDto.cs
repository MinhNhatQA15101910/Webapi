namespace Webapi.SharedKernel.DTOs.Product;

public class UpdateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int InStock { get; set; }
    public ICollection<Guid> CategoryIds { get; set; } = new List<Guid>();
}
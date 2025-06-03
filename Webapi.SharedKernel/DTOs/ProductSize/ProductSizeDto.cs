namespace Webapi.SharedKernel.DTOs.ProductSize;

public class ProductSizeDto
{
    public Guid Id { get; set; }
    public string Size { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public Guid? ProductId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
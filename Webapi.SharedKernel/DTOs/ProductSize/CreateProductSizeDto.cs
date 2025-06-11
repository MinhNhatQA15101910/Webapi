namespace Webapi.SharedKernel.DTOs.ProductSize;

public class CreateProductSizeDto
{
    public string Size { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public Guid? ProductId { get; set; }
}
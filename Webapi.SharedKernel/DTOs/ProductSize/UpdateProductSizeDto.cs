namespace Webapi.SharedKernel.DTOs.ProductSize;

public class UpdateProductSizeDto
{
    public Guid Id { get; set; }
    public string Size { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
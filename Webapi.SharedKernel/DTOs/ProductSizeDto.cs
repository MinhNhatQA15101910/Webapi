namespace Webapi.SharedKernel.DTOs;

public class ProductSizeDto
{
    public Guid Id { get; set; }
    public string Size { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public Guid? ProductId { get; set; } // Make nullable
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateProductSizeDto
{
    public string Size { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public Guid? ProductId { get; set; } // Make nullable
}

public class UpdateProductSizeDto
{
    public string Size { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public Guid? ProductId { get; set; } // Make nullable
}
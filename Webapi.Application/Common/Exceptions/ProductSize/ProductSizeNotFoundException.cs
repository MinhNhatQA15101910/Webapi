namespace Webapi.Application.Common.Exceptions.ProductSize;

public class ProductSizeNotFoundException : NotFoundException
{
    public ProductSizeNotFoundException(Guid sizeId)
        : base($"The product size with the identifier {sizeId} was not found.")
    {
    }
    
    public ProductSizeNotFoundException(Guid productId, string size)
        : base($"The size '{size}' was not found for product {productId}.")
    {
    }
    
    public ProductSizeNotFoundException(string message)
        : base(message)
    {
    }
}
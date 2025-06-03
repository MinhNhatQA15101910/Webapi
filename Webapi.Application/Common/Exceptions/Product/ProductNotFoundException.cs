namespace Webapi.Application.Common.Exceptions.Product;

public class ProductNotFoundException : NotFoundException
{
    public ProductNotFoundException(Guid productId)
        : base($"The product with the identifier {productId} was not found.")
    {
    }
    
    public ProductNotFoundException(string message)
        : base(message)
    {
    }
}
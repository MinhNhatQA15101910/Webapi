namespace Webapi.Application.Common.Exceptions.Product;

public class ProductUpdateException : BadRequestException
{
    public ProductUpdateException(Guid productId, string message)
        : base($"Failed to update product {productId}: {message}")
    {
    }

    public ProductUpdateException(Guid productId)
        : base($"An error occurred while updating product {productId}.")
    {
    }
}
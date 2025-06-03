namespace Webapi.Application.Common.Exceptions.Product;

public class ProductDeleteException : BadRequestException
{
    public ProductDeleteException(Guid productId, string message)
        : base($"Failed to delete product {productId}: {message}")
    {
    }

    public ProductDeleteException(Guid productId)
        : base($"An error occurred while deleting product {productId}.")
    {
    }
}
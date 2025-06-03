namespace Webapi.Application.Common.Exceptions.ProductSize;

public class ProductSizeDeleteException : BadRequestException
{
    public ProductSizeDeleteException(Guid sizeId, string message)
        : base($"Failed to delete product size {sizeId}: {message}")
    {
    }

    public ProductSizeDeleteException(Guid sizeId)
        : base($"An error occurred while deleting product size {sizeId}.")
    {
    }
}
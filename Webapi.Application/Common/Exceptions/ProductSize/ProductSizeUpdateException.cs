namespace Webapi.Application.Common.Exceptions.ProductSize;

public class ProductSizeUpdateException : BadRequestException
{
    public ProductSizeUpdateException(Guid sizeId, string message)
        : base($"Failed to update product size {sizeId}: {message}")
    {
    }

    public ProductSizeUpdateException(Guid sizeId)
        : base($"An error occurred while updating product size {sizeId}.")
    {
    }
}
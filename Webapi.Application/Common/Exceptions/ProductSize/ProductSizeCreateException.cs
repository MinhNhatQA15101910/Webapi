namespace Webapi.Application.Common.Exceptions.ProductSize;

public class ProductSizeCreateException : BadRequestException
{
    public ProductSizeCreateException(string message)
        : base($"Failed to create product size: {message}")
    {
    }

    public ProductSizeCreateException()
        : base("An error occurred while creating the product size.")
    {
    }
}
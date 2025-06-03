namespace Webapi.Application.Common.Exceptions.Product;

public class ProductCreateException : BadRequestException
{
    public ProductCreateException(string message)
        : base($"Failed to create product: {message}")
    {
    }

    public ProductCreateException()
        : base("An error occurred while creating the product.")
    {
    }
}
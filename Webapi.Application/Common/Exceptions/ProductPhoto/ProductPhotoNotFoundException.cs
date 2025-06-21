namespace Webapi.Application.Common.Exceptions.ProductPhoto;

public class ProductPhotoNotFoundException : NotFoundException
{
    public ProductPhotoNotFoundException(Guid photoId)
        : base($"The product photo with the identifier {photoId} was not found.")
    {
    }
    
    public ProductPhotoNotFoundException(Guid productId, Guid photoId)
        : base($"The photo with ID {photoId} was not found for product {productId}.")
    {
    }
    
    public ProductPhotoNotFoundException(string message)
        : base(message)
    {
    }
}
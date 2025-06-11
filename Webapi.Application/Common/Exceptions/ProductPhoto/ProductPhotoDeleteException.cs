namespace Webapi.Application.Common.Exceptions.ProductPhoto;

public class ProductPhotoDeleteException : BadRequestException
{
    public ProductPhotoDeleteException(Guid photoId, string message)
        : base($"Failed to delete product photo {photoId}: {message}")
    {
    }

    public ProductPhotoDeleteException(Guid photoId)
        : base($"An error occurred while deleting product photo {photoId}.")
    {
    }
}
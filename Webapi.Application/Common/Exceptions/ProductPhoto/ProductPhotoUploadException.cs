namespace Webapi.Application.Common.Exceptions.ProductPhoto;

public class ProductPhotoUploadException : BadRequestException
{
    public ProductPhotoUploadException(string message)
        : base($"Failed to upload product photo: {message}")
    {
    }

    public ProductPhotoUploadException()
        : base("An error occurred while uploading the product photo.")
    {
    }
}
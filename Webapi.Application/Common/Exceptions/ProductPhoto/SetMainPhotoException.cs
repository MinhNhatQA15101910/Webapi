namespace Webapi.Application.Common.Exceptions.ProductPhoto;

public class SetMainPhotoException : BadRequestException
{
    public SetMainPhotoException(Guid photoId, string message)
        : base($"Failed to set photo {photoId} as main: {message}")
    {
    }

    public SetMainPhotoException(Guid photoId)
        : base($"An error occurred while setting photo {photoId} as main.")
    {
    }
}
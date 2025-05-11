using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Webapi.Application.Common.Interfaces.Services;

public interface IFileService
{
    Task<DeletionResult> DeleteFileAsync(string publicId, ResourceType resourceType);
    Task<ImageUploadResult> UploadPhotoAsync(string folderPath, IFormFile file);
}

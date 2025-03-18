using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IFileService
{
    Task<ImageUploadResult> UploadPhotoAsync(string folderPath, IFormFile file);
}

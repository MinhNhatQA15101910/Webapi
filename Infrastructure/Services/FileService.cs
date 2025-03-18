using Application.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Services;

public class FileService : IFileService
{
    private readonly Cloudinary _cloudinary;
    private readonly string _folderRoot = "Webapi/";

    public FileService(IOptions<CloudinarySettings> config)
    {
        var acc = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );

        _cloudinary = new Cloudinary(acc);
    }

    public async Task<ImageUploadResult> UploadPhotoAsync(string folderPath, IFormFile file)
    {
        var uploadResult = new ImageUploadResult();

        if (file.Length > 0)
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = _folderRoot + folderPath
            };

            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }

        return uploadResult;
    }
}

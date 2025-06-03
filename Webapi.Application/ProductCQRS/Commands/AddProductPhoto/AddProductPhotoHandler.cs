using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions.Product;
using Webapi.Application.Common.Exceptions.ProductPhoto;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Application.Common.Interfaces.Services;
using Webapi.Application.ProductCQRS.Commands.AddProductPhoto;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.DTOs.ProductPhoto;

namespace Webapi.Application.ProductCQRS.Commands.AddProductPhoto;
public class AddProductPhotoHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IFileService fileService,
    IMapper mapper
) : ICommandHandler<AddProductPhotoCommand, ProductPhotoDto>
{
    public async Task<ProductPhotoDto> Handle(AddProductPhotoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = httpContextAccessor.HttpContext.User.GetUserId();
            
            // Verify product exists
            var product = await unitOfWork.ProductRepository.GetByIdAsync(request.ProductId, cancellationToken)
                ?? throw new ProductNotFoundException(request.ProductId);
                
            // Upload photo to cloud storage
            var folderPath = $"products/{request.ProductId}";
            var uploadResult = await fileService.UploadPhotoAsync(folderPath, request.File);
            
            if (uploadResult.Error != null)
            {
                throw new ProductPhotoUploadException($"Failed to upload photo: {uploadResult.Error.Message}");
            }
            
            // Create new photo entity
            var photo = new ProductPhoto
            {
                Url = uploadResult.SecureUrl.AbsoluteUri,
                PublicId = uploadResult.PublicId,
                IsMain = false, // New photos are not main by default
                ProductId = request.ProductId
            };
            
            // Set as main if this is the first photo
            var existingPhotos = await unitOfWork.ProductRepository.GetProductPhotosAsync(request.ProductId, cancellationToken);
            if (!existingPhotos.Any())
            {
                photo.IsMain = true;
            }
            
            // Add photo to repository
            unitOfWork.ProductPhotoRepository.Add(photo);
            await unitOfWork.CompleteAsync();
            
            // Return mapped DTO
            return mapper.Map<ProductPhotoDto>(photo);
        }
        catch (ProductNotFoundException)
        {
            // Rethrow specific exceptions
            throw;
        }
        catch (ProductPhotoUploadException)
        {
            // Rethrow specific exceptions
            throw;
        }
        catch (Exception ex)
        {
            // Wrap other exceptions
            throw new ProductPhotoUploadException($"An unexpected error occurred: {ex.Message}");
        }
    }
}
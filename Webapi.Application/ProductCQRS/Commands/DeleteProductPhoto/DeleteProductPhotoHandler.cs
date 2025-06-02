using AutoMapper;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Application.Common.Interfaces.Services;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;

namespace Webapi.Application.ProductCQRS.Commands.DeleteProductPhoto;

public class DeleteProductPhotoHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IFileService fileService,
    IMapper mapper
) : ICommandHandler<DeleteProductPhotoCommand, ProductPhoto>
{
    public async Task<ProductPhoto> Handle(DeleteProductPhotoCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.User.GetUserId();
        
        // Check if product exists
        var product = await unitOfWork.ProductRepository.GetByIdAsync(
            request.ProductId, cancellationToken)
            ?? throw new Exception( $"Product with id: {request.ProductId} was not found");
        
        // Check if photo exists and belongs to the product
        var photo = await unitOfWork.ProductRepository.GetPhotoByIdAsync(
            request.PhotoId, cancellationToken)
            ?? throw new Exception($"Photo with ID {request.PhotoId} not found");
            
        if (photo.ProductId != request.ProductId)
        {
            throw new Exception($"Photo with ID {request.PhotoId} not found for product {request.ProductId}");
        }
        
        // Delete from Cloudinary if it has a PublicId
        if (!string.IsNullOrEmpty(photo.PublicId))
        {
            var result = await fileService.DeleteFileAsync(
                photo.PublicId, 
                CloudinaryDotNet.Actions.ResourceType.Image
            );
            
            if (result.Error != null)
            {
                throw new BadRequestException($"Failed to delete photo: {result.Error.Message}");
            }
        }
        
        // Store a copy of the photo to return
        var deletedPhoto = photo;
        
        // Delete from database
        await unitOfWork.ProductRepository.DeletePhotoAsync(photo.Id, cancellationToken);
        
        return deletedPhoto;
    }
}
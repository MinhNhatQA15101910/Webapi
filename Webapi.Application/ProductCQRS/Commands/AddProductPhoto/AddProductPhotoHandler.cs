using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Application.Common.Interfaces.Services;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;

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
        var userId = httpContextAccessor.HttpContext?.User.GetUserId();
        
        // Check if product exists
        var product = await unitOfWork.ProductRepository.GetByIdAsync(
            request.ProductId, cancellationToken)
            ?? throw new Exception( $"Product with id: {request.ProductId} was not found");
        
        // Upload photo to Cloudinary - using the correct folder path for products
        var result = await fileService.UploadPhotoAsync(
            "products",  // Specify the folder path for product images
            request.File
        );
        
        if (result.Error != null)
        {
            throw new BadRequestException($"Failed to upload photo: {result.Error.Message}");
        }
        
        // Create product photo entity
        var photo = new ProductPhoto
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
            ProductId = request.ProductId
        };
        
        // Add photo to database
        await unitOfWork.ProductRepository.AddPhotoAsync(request.ProductId, photo, cancellationToken);
        
        // Return DTO
        return mapper.Map<ProductPhotoDto>(photo);
    }
}
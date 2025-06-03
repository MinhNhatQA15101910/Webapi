using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;

namespace Webapi.Application.ProductCQRS.Commands.SetMainProductPhoto;

public class SetMainProductPhotoHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork
) : ICommandHandler<SetMainProductPhotoCommand, ProductPhoto>
{
    public async Task<ProductPhoto> Handle(SetMainProductPhotoCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.User.GetUserId();
        
        // Check if product exists
        var product = await unitOfWork.ProductRepository.GetByIdAsync(
            request.ProductId, cancellationToken)
            ?? throw new Exception( $"Product with id: {request.ProductId} was not found." );
        
        // Check if photo exists and belongs to the product
        var photo = await unitOfWork.ProductRepository.GetPhotoByIdAsync(
            request.PhotoId, cancellationToken);
            
        if (photo == null || photo.ProductId != request.ProductId)
        {
            throw new Exception($"Photo with ID {request.PhotoId} not found for product {request.ProductId}");
        }
        
        // Set as main photo
        await unitOfWork.ProductRepository.SetMainPhotoAsync(
            request.ProductId, request.PhotoId, cancellationToken);

        return await unitOfWork.ProductRepository.GetPhotoByIdAsync(
            request.PhotoId, cancellationToken) ?? throw new Exception($"Photo with ID {request.PhotoId} not found");
    }
}
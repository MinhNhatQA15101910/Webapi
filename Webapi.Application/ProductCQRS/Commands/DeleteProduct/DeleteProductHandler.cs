using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Application.ProductCQRS.Commands.DeleteProduct;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;

public class DeleteProductHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<DeleteProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.User.GetUserId();
        
        // Check if user has permission to delete the product (optional)
        // This depends on your authorization strategy
        
        // Get product
        var product = await unitOfWork.ProductRepository.GetByIdAsync(
            request.ProductId, cancellationToken) 
            ?? throw new Exception( $"Product with id: {request.ProductId} was not found");
        
        // Delete photos first (if you want to clean up related resources)
        var photos = await unitOfWork.ProductRepository.GetProductPhotosAsync(
            product.Id, cancellationToken);
            
        foreach (var photo in photos)
        {
            await unitOfWork.ProductRepository.DeletePhotoAsync(photo.Id, cancellationToken);
        }
        
        // Delete the product
        unitOfWork.ProductRepository.Delete(product);
        
        // Save changes
        await unitOfWork.CompleteAsync();
        
        return mapper.Map<ProductDto>(product);
    }
}
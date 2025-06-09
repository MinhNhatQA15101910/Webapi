using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions.Category;
using Webapi.Application.Common.Exceptions.Product;
using Webapi.Application.Common.Exceptions.ProductPhoto;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Application.Common.Interfaces.Services;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.DTOs.Product;

namespace Webapi.Application.ProductCQRS.Commands.UpdateProduct;

public class UpdateProductHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IFileService fileService
) : ICommandHandler<UpdateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get existing product
            var product = await unitOfWork.ProductRepository.GetProductWithDetailsAsync(request.ProductId, cancellationToken)
                ?? throw new ProductNotFoundException(request.ProductId);

            // Update product properties
            product.Name = request.ProductDto.Name;
            product.Description = request.ProductDto.Description;
            product.Price = request.ProductDto.Price;
            product.InStock = request.ProductDto.InStock;
            product.UpdatedAt = DateTime.UtcNow;

            // Update categories if needed
            if (request.ProductDto.CategoryIds != null)
            {
                // Get current categories
                var currentCategories = product.Categories.Select(pc => pc.CategoryId).ToList();
                
                // Categories to remove (in current but not in request)
                var categoriesToRemove = currentCategories
                    .Where(categoryId => !request.ProductDto.CategoryIds.Contains(categoryId))
                    .ToList();
                
                // Categories to add (in request but not in current)
                var categoriesToAdd = request.ProductDto.CategoryIds
                    .Where(categoryId => !currentCategories.Contains(categoryId))
                    .ToList();
                
                // Remove categories
                foreach (var categoryId in categoriesToRemove)
                {
                    await unitOfWork.ProductRepository.RemoveCategoryAsync(product.Id, categoryId, cancellationToken);
                }
                
                // Add categories
                foreach (var categoryId in categoriesToAdd)
                {
                    var category = await unitOfWork.CategoryRepository.GetByIdAsync(categoryId, cancellationToken)
                        ?? throw new CategoryNotFoundException(categoryId);
                        
                    await unitOfWork.ProductRepository.AddCategoryAsync(product.Id, categoryId, cancellationToken);
                }
            }
            
            // Handle photo updates
            await HandlePhotoUpdates(request, product.Id, cancellationToken);
            
            // Handle product size updates
            await HandleProductSizeUpdates(request, product.Id, cancellationToken);
            
            // Update product in repository
            unitOfWork.ProductRepository.Update(product);
            
            // Save changes
            await unitOfWork.CompleteAsync();
            
            // Get the updated product with all related data
            var updatedProduct = await unitOfWork.ProductRepository.GetProductWithDetailsAsync(product.Id, cancellationToken)
                ?? throw new ProductNotFoundException(product.Id);
                
            // Map to DTO and return
            return mapper.Map<ProductDto>(updatedProduct);
        }
        catch (ProductNotFoundException)
        {
            // Rethrow these specific exceptions to maintain their status code
            throw;
        }
        catch (CategoryNotFoundException)
        {
            // Rethrow these specific exceptions to maintain their status code
            throw;
        }
        catch (ProductPhotoUploadException)
        {
            // Rethrow these specific exceptions to maintain their status code
            throw;
        }
        catch (Exception ex)
        {
            // Catch any other unexpected exceptions
            throw new ProductUpdateException(request.ProductId, ex.Message);
        }
    }
    
    private async Task HandlePhotoUpdates(UpdateProductCommand request, Guid productId, CancellationToken cancellationToken)
    {
        // 1. Remove photos if specified
        if (request.ProductDto.PhotoIdsToRemove?.Any() == true)
        {
            foreach (var photoId in request.ProductDto.PhotoIdsToRemove)
            {
                var photo = await unitOfWork.ProductRepository.GetPhotoByIdAsync(photoId, cancellationToken);
                
                if (photo != null && photo.ProductId == productId)
                {
                    
                    // Delete from database
                    await unitOfWork.ProductRepository.DeletePhotoAsync(photoId, cancellationToken);
                }
            }
        }
        
        // 2. Set main photo if specified
        if (request.ProductDto.MainPhotoId.HasValue)
        {
            await unitOfWork.ProductRepository.SetMainPhotoAsync(
                productId, request.ProductDto.MainPhotoId.Value, cancellationToken);
        }
        
        // 3. Upload new main image if provided
        if (request.ProductDto.MainImage != null)
        {
            var folderPath = $"products/{productId}";
            var uploadResult = await fileService.UploadPhotoAsync(folderPath, request.ProductDto.MainImage);
            
            if (uploadResult.Error != null)
            {
                throw new ProductPhotoUploadException($"Failed to upload main image: {uploadResult.Error.Message}");
            }
            
            // Create new photo entity
            var photo = new ProductPhoto
            {
                Url = uploadResult.SecureUrl.AbsoluteUri,
                PublicId = uploadResult.PublicId,
                IsMain = true, // Set as main
                ProductId = productId
            };
            
            // Clear main flag from other photos
            var currentMainPhoto = await unitOfWork.ProductRepository.GetMainPhotoAsync(productId, cancellationToken);
            if (currentMainPhoto != null)
            {
                currentMainPhoto.IsMain = false;
            }
            
            // Add new photo
            unitOfWork.ProductPhotoRepository.Add(photo);
        }
        
        // 4. Upload additional images if provided
        if (request.ProductDto.AdditionalImages?.Any() == true)
        {
            foreach (var image in request.ProductDto.AdditionalImages)
            {
                try
                {
                    var folderPath = $"products/{productId}";
                    var uploadResult = await fileService.UploadPhotoAsync(folderPath, image);
                    
                    if (uploadResult.Error != null)
                    {
                        // Log the error but continue with other images
                        Console.WriteLine($"Failed to upload additional image: {uploadResult.Error.Message}");
                        continue;
                    }
                    
                    var photo = new ProductPhoto
                    {
                        Url = uploadResult.SecureUrl.AbsoluteUri,
                        PublicId = uploadResult.PublicId,
                        IsMain = false,
                        ProductId = productId
                    };
                    
                    unitOfWork.ProductPhotoRepository.Add(photo);
                }
                catch (Exception ex)
                {
                    // Log the error but continue with other images
                    Console.WriteLine($"Error processing additional image: {ex.Message}");
                }
            }
        }
    }
    
    // Add this method to your UpdateProductHandler class
    private async Task HandleProductSizeUpdates(UpdateProductCommand request, Guid productId, CancellationToken cancellationToken)
    {
        // 1. Remove sizes if specified
        if (request.ProductDto.SizeIdsToRemove?.Any() == true)
        {
            foreach (var sizeId in request.ProductDto.SizeIdsToRemove)
            {
                var size = await unitOfWork.ProductSizeRepository.GetByIdAsync(sizeId, cancellationToken);
                
                if (size != null && size.ProductId == productId)
                {
                    unitOfWork.ProductSizeRepository.Remove(size);
                }
            }
        }
        
        // 2. Update existing sizes
        if (request.ProductDto.SizesToUpdate?.Any() == true)
        {
            foreach (var sizeUpdate in request.ProductDto.SizesToUpdate)
            {
                // Make sure we're only updating sizes that belong to this product
                var existingSize = await unitOfWork.ProductSizeRepository.GetByIdAsync(sizeUpdate.Id, cancellationToken);
                
                if (existingSize != null && existingSize.ProductId == productId)
                {
                    existingSize.Size = sizeUpdate.Size;
                    existingSize.Quantity = sizeUpdate.Quantity;
                    existingSize.UpdatedAt = DateTime.UtcNow;
                    
                    unitOfWork.ProductSizeRepository.Update(existingSize);
                }
            }
        }
        
        // 3. Add new sizes
        if (request.ProductDto.SizesToAdd?.Any() == true)
        {
            foreach (var newSize in request.ProductDto.SizesToAdd)
            {
                var productSize = new ProductSize
                {
                    Size = newSize.Size,
                    Quantity = newSize.Quantity,
                    ProductId = productId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                
                unitOfWork.ProductSizeRepository.Add(productSize);
            }
        }
    }
}
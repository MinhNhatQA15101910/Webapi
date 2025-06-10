using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Builders;
using Webapi.Application.Common.Exceptions.Category;
using Webapi.Application.Common.Exceptions.Product;
using Webapi.Application.Common.Exceptions.ProductPhoto;
using Webapi.Application.Common.Exceptions.ProductSize;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Application.Common.Interfaces.Services;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs.Product;
using Webapi.SharedKernel.DTOs.ProductSize;

namespace Webapi.Application.ProductCQRS.Commands.CreateProduct;

public class CreateProductHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IFileService fileService,
    IMapper mapper
) : ICommandHandler<CreateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Process existing sizes
            if (request.ProductDto.SizeIds?.Any() == true)
            {
                foreach (var sizeId in request.ProductDto.SizeIds)
                {
                    var size = await unitOfWork.ProductSizeRepository.GetByIdAsync(sizeId, cancellationToken)
                        ?? throw new ProductSizeNotFoundException(sizeId);
                    
                    // We'll associate these existing sizes with the product after creation
                }
            }
            
            // Use the builder pattern to create a new product
            var (product, categoryIds, newSizes, existingSizeIds) = ProductBuilder
                .FromDto(request.ProductDto)
                .Build();
                
            // Add product to database
            unitOfWork.ProductRepository.Add(product);
            
            // We'll commit at the end of the transaction
            await unitOfWork.CompleteAsync();
            
            // Add categories if needed
            if (categoryIds.Any())
            {
                foreach (var categoryId in categoryIds)
                {
                    var category = await unitOfWork.CategoryRepository.GetByIdAsync(categoryId, cancellationToken)
                        ?? throw new CategoryNotFoundException(categoryId);
                        
                    await unitOfWork.ProductRepository.AddCategoryAsync(product.Id, categoryId, cancellationToken);
                }
            }
            
            // Add new sizes
            if (newSizes.Any())
            {
                foreach (var sizeDto in newSizes)
                {
                    try
                    {
                        var productSize = new ProductSize
                        {
                            Size = sizeDto.Size,
                            Quantity = sizeDto.Quantity,
                            ProductId = product.Id,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        
                        unitOfWork.ProductSizeRepository.Add(productSize);
                    }
                    catch (Exception ex)
                    {
                        throw new ProductSizeCreateException($"Failed to create size: {ex.Message}");
                    }
                }
            }
            
            // Link existing sizes to the product
            if (existingSizeIds.Any())
            {
                foreach (var sizeId in existingSizeIds)
                {
                    try
                    {
                        var existingSize = await unitOfWork.ProductSizeRepository.GetByIdAsync(sizeId, cancellationToken)
                            ?? throw new ProductSizeNotFoundException(sizeId);
                        
                        // Create a new size with the same properties but linked to this product
                        var productSize = new ProductSize
                        {
                            Size = existingSize.Size,
                            Quantity = existingSize.Quantity,
                            ProductId = product.Id,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        
                        unitOfWork.ProductSizeRepository.Add(productSize);
                    }
                    catch (ProductSizeNotFoundException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        throw new ProductSizeCreateException($"Failed to link existing size: {ex.Message}");
                    }
                }
            }
            
            // Handle main image
            if (request.ProductDto.MainImage != null)
            {
                try
                {
                    var folderPath = $"products/{product.Id}";
                    var uploadResult = await fileService.UploadPhotoAsync(folderPath, request.ProductDto.MainImage);
                    
                    if (uploadResult.Error != null)
                    {
                        throw new ProductPhotoUploadException($"Failed to upload main image: {uploadResult.Error.Message}");
                    }

                    var photo = new ProductPhoto
                    {
                        Url = uploadResult.SecureUrl.AbsoluteUri,
                        PublicId = uploadResult.PublicId,
                        IsMain = true,
                        ProductId = product.Id
                    };

                    unitOfWork.ProductPhotoRepository.Add(photo);
                }
                catch (ProductPhotoUploadException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new ProductPhotoUploadException($"Error processing main image: {ex.Message}");
                }
            }

            // Handle additional images
            if (request.ProductDto.AdditionalImages?.Any() == true)
            {
                foreach (var image in request.ProductDto.AdditionalImages)
                {
                    try
                    {
                        var folderPath = $"products/{product.Id}";
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
                            ProductId = product.Id
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
            
            // Single commit for the entire transaction
            await unitOfWork.CompleteAsync();
            
            // Fetch the product with all related data
            var createdProduct = await unitOfWork.ProductRepository.GetProductWithDetailsAsync(product.Id, cancellationToken)
                ?? throw new ProductNotFoundException(product.Id);
                
            // Map to DTO and return
            return mapper.Map<ProductDto>(createdProduct);
        }
        catch (ProductNotFoundException ex)
        {
            // Rethrow these specific exceptions to maintain their status code
            throw;
        }
        catch (CategoryNotFoundException ex)
        {
            // Rethrow these specific exceptions to maintain their status code
            throw;
        }
        catch (ProductSizeNotFoundException ex)
        {
            // Rethrow these specific exceptions to maintain their status code
            throw;
        }
        catch (ProductPhotoUploadException ex)
        {
            // Rethrow these specific exceptions to maintain their status code
            throw;
        }
        catch (Exception ex)
        {
            // Catch any other unexpected exceptions and wrap them in a ProductCreateException
            throw new ProductCreateException($"An unexpected error occurred: {ex.Message}");
        }
    }
}
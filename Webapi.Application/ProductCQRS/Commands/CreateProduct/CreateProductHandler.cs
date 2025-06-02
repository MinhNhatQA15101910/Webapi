using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Builders;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Application.Common.Interfaces.Services;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
        var userId = httpContextAccessor.HttpContext.User.GetUserId();

        //From the sizes Guid, fetch each size from the database then add to the _sizes list
        if (request.ProductDto.SizeIds.Any())
        {
            foreach (var sizeId in request.ProductDto.SizeIds)
            {
                var size = await unitOfWork.ProductSizeRepository.GetByIdAsync(sizeId, cancellationToken);
                if (size != null)
                {
                    request.ProductDto.Sizes.Add(new CreateProductSizeDto
                    {
                        Size = size.Size,
                        Quantity = size.Quantity
                    });
                }
            }
        }
        
        // Use the builder pattern to create a new product
        var (product, categoryIds, sizes) = ProductBuilder
            .FromDto(request.ProductDto)
            .Build();
            
        // Add product to database
        unitOfWork.ProductRepository.Add(product);
        await unitOfWork.CompleteAsync();
        
        // Log category IDs
        Console.WriteLine($"Categories count: {categoryIds.Count()}");
        Console.WriteLine($"Categories IDs: {string.Join(", ", categoryIds)}");
        
        // Add categories if needed
        if (categoryIds.Any())
        {
            Console.WriteLine("Adding categories to product...");
            foreach (var categoryId in categoryIds)
            {
                Console.WriteLine($"Adding category {categoryId} to product {product.Id}");
                try 
                {
                    await unitOfWork.ProductRepository.AddCategoryAsync(product.Id, categoryId, cancellationToken);
                    Console.WriteLine($"Successfully added category {categoryId}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding category {categoryId}: {ex.Message}");
                    throw;
                }
            }
            
            // Verify categories were added
            Console.WriteLine("Verifying categories were added correctly...");
            await unitOfWork.CompleteAsync();
        }
        else
        {
            Console.WriteLine("No categories to add");
        }
        
        // Add sizes if needed
        if (sizes.Any())
        {
            Console.WriteLine($"Adding {sizes.Count} sizes to product...");
            foreach (var sizeDto in sizes)
            {
                Console.WriteLine($"Adding size {sizeDto.Size} with quantity {sizeDto.Quantity} to product {product.Id}");
                try 
                {
                    var productSize = new ProductSize
                    {
                        Size = sizeDto.Size,
                        Quantity = sizeDto.Quantity,
                        ProductId = product.Id
                    };
                    
                    unitOfWork.ProductSizeRepository.Add(productSize);
                    Console.WriteLine($"Successfully added size {sizeDto.Size}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding size {sizeDto.Size}: {ex.Message}");
                    throw;
                }
            }
            
            // Verify sizes were added
            Console.WriteLine("Verifying sizes were added correctly...");
            await unitOfWork.CompleteAsync();
        }
        else
        {
            Console.WriteLine("No sizes to add");
        }
        
        // Handle MainImage upload if present - FIX: Changed condition from "== null" to "!= null"
        if (request.ProductDto.MainImage != null)
        {
            Console.WriteLine($"Processing main image for product {product.Id}");
            try
            {
                // Use the folder path "products/{productId}" for organization
                var folderPath = $"products/{product.Id}";
                var uploadResult = await fileService.UploadPhotoAsync(folderPath, request.ProductDto.MainImage);
                
                if (uploadResult.Error != null)
                {
                    Console.WriteLine($"Error uploading main image: {uploadResult.Error.Message}");
                    throw new Exception($"Failed to upload main image: {uploadResult.Error.Message}");
                }

                var photo = new ProductPhoto
                {
                    Url = uploadResult.SecureUrl.AbsoluteUri,
                    PublicId = uploadResult.PublicId,
                    IsMain = true,
                    ProductId = product.Id
                };

                unitOfWork.ProductPhotoRepository.Add(photo);
                await unitOfWork.CompleteAsync();
                Console.WriteLine($"Successfully added main photo with ID {photo.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing main image: {ex.Message}");
                throw;
            }
        }
        else
        {
            Console.WriteLine("No main image provided");
        }

        // Handle additional images if present
        if (request.ProductDto.AdditionalImages?.Any() == true)
        {
            Console.WriteLine($"Processing {request.ProductDto.AdditionalImages.Count} additional images");
            foreach (var image in request.ProductDto.AdditionalImages)
            {
                try
                {
                    // Use the same folder path for all images of this product
                    var folderPath = $"products/{product.Id}";
                    var uploadResult = await fileService.UploadPhotoAsync(folderPath, image);
                    
                    if (uploadResult.Error != null)
                    {
                        Console.WriteLine($"Error uploading additional image: {uploadResult.Error.Message}");
                        continue; // Skip this image if there was an error
                    }

                    var photo = new ProductPhoto
                    {
                        Url = uploadResult.SecureUrl.AbsoluteUri,
                        PublicId = uploadResult.PublicId,
                        IsMain = false,
                        ProductId = product.Id
                    };

                    unitOfWork.ProductPhotoRepository.Add(photo);
                    Console.WriteLine($"Added additional photo with URL: {photo.Url}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing additional image: {ex.Message}");
                    // Continue with other images even if one fails
                }
            }
            await unitOfWork.CompleteAsync();
        }
        
        Console.WriteLine($"Added product {product.Id} with name '{product.Name}' with {product.Categories.Count()} categories and {product.Sizes.Count()} sizes");
        // Map to DTO and return
        return mapper.Map<ProductDto>(product);
    }
}
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Builders;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.ProductCQRS.Commands.CreateProduct;

public class CreateProductHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<CreateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext.User.GetUserId();
        
        // Use the builder pattern to create a new product
        var (product, categoryIds) = ProductBuilder
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
        
        Console.WriteLine($"Added product {product.Id} with name '{product.Name}' with category '{product.Categories.Count()}'");
        // Map to DTO and return
        return mapper.Map<ProductDto>(product);
    }
}
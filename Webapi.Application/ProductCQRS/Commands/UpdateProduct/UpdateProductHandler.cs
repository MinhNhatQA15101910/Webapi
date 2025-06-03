using AutoMapper;
using Webapi.Application.Common.Exceptions.Category;
using Webapi.Application.Common.Exceptions.Product;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.ProductCQRS.Commands.UpdateProduct;

public class UpdateProductHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper
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
        catch (Exception ex)
        {
            // Catch any other unexpected exceptions
            throw new ProductUpdateException(request.ProductId, ex.Message);
        }
    }
}
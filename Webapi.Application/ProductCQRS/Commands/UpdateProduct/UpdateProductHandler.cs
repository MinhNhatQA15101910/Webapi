using AutoMapper;
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
        // Get product with details
        var product = await unitOfWork.ProductRepository.GetProductWithDetailsAsync(
            request.ProductId, cancellationToken)
            ?? throw new Exception($"Product with id: {request.ProductId} was not found");

        // Update basic properties
        product.Name = request.ProductDto.Name;
        product.Description = request.ProductDto.Description;
        product.Price = request.ProductDto.Price;
        product.InStock = request.ProductDto.InStock;
        product.UpdatedAt = DateTime.UtcNow;

        // Update the product
        unitOfWork.ProductRepository.Update(product);

        // Get existing category IDs
        var existingCategoryIds = product.Categories.Select(pc => pc.CategoryId).ToList();

        // Add new categories
        foreach (var categoryId in request.ProductDto.CategoryIds.Except(existingCategoryIds))
        {
            await unitOfWork.ProductRepository.AddCategoryAsync(
                product.Id, categoryId, cancellationToken);
        }

        // Remove categories that are no longer associated
        foreach (var categoryId in existingCategoryIds.Except(request.ProductDto.CategoryIds))
        {
            await unitOfWork.ProductRepository.RemoveCategoryAsync(
                product.Id, categoryId, cancellationToken);
        }

        await unitOfWork.CompleteAsync();

        // Fetch the updated product with all related data
        var updatedProduct = await unitOfWork.ProductRepository
            .GetProductWithDetailsAsync(request.ProductId, cancellationToken)
            ?? throw new Exception($"Product with id: {request.ProductId} was not found");

        // Map to DTO and return
        return mapper.Map<ProductDto>(updatedProduct);
    }
}

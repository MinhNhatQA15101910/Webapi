using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Builders;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.ProductsCQRS.Commands.CreateProduct;

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
        
        // Add categories if needed
        if (categoryIds.Any())
        {
            foreach (var categoryId in categoryIds)
            {
                await unitOfWork.ProductRepository.AddCategoryAsync(product.Id, categoryId, cancellationToken);
            }
        }
        
        // Map to DTO and return
        return mapper.Map<ProductDto>(product);
    }
}
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.ProductSizeCQRS.Commands.CreateProductSize;

public class CreateProductSizeHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<CreateProductSizeCommand, ProductSizeDto>
{
    public async Task<ProductSizeDto> Handle(CreateProductSizeCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext.User.GetUserId();
        
        // First check if the product exists
        if (request.ProductSizeDto.ProductId.HasValue)
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync(request.ProductSizeDto.ProductId.Value, cancellationToken);
            if (product == null)
            {
                throw new Exception($"Product with ID {request.ProductSizeDto.ProductId.Value} not found");
            }
        }      
        // Create new product size
        var productSize = mapper.Map<ProductSize>(request.ProductSizeDto);
        
        // Set creation timestamp
        productSize.CreatedAt = DateTime.UtcNow;
        productSize.UpdatedAt = DateTime.UtcNow;
        
        // Add to repository
        unitOfWork.ProductSizeRepository.Add(productSize);
        await unitOfWork.CompleteAsync();
        
        // Return mapped DTO
        return mapper.Map<ProductSizeDto>(productSize);
    }
}
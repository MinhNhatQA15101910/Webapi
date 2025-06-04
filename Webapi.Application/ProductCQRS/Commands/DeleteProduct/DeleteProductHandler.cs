using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions.Product;
using Webapi.Application.Common.Exceptions.ProductPhoto;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Application.Common.Interfaces.Services;
using Webapi.Application.ProductCQRS.Commands.DeleteProduct;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.DTOs.Product;

public class DeleteProductHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IFileService fileService,
    IMapper mapper
) : ICommandHandler<DeleteProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get product with all related data
            var product = await unitOfWork.ProductRepository.GetProductWithDetailsAsync(request.ProductId, cancellationToken)
                ?? throw new ProductNotFoundException(request.ProductId);
                
            // Map to DTO for return value before deletion
            var productDto = mapper.Map<ProductDto>(product);
            
            // Delete product
            unitOfWork.ProductRepository.Delete(product);
            
            // Save changes
            await unitOfWork.CompleteAsync();
            
            return productDto;
        }
        catch (ProductNotFoundException)
        {
            // Rethrow these specific exceptions to maintain their status code
            throw;
        }
        catch (Exception ex)
        {
            // Catch any other unexpected exceptions
            throw new ProductDeleteException(request.ProductId, ex.Message);
        }
    }
}
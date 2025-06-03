using AutoMapper;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Exceptions.Product;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.ProductCQRS.Queries.GetProductById;

public class GetProductByIdHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IQueryHandler<GetProductByIdQuery, ProductDto>
{
    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        // Get product with details
        var product = await unitOfWork.ProductRepository.GetProductWithDetailsAsync(
            request.ProductId, cancellationToken)
            ?? throw new ProductNotFoundException(request.ProductId);

        // Map to DTO and return
        return mapper.Map<ProductDto>(product);
    }
}

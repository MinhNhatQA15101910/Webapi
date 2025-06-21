using AutoMapper;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.DTOs.Product;
using Webapi.SharedKernel.Helpers;

namespace Webapi.Application.ProductCQRS.Queries.GetProducts;

public class GetProductsHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IQueryHandler<GetProductsQuery, PagedList<ProductDto>>
{
    public async Task<PagedList<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        // Get products with pagination
        var productsPage = await unitOfWork.ProductRepository.GetProductsAsync(
            request.ProductParams, cancellationToken);

        // Map products to DTOs
        var productDtos = productsPage.Select(mapper.Map<ProductDto>).ToList();

        // Create new PagedList with mapped items
        return new PagedList<ProductDto>(
            productDtos,
            productsPage.TotalCount,
            productsPage.CurrentPage,
            productsPage.PageSize
        );
    }
}

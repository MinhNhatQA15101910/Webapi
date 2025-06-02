using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.Helpers;

namespace Webapi.Application.ProductCQRS.Queries.GetProducts;

public class GetProductsHandler(
    IHttpContextAccessor httpContextAccessor,
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
        var productDtos = productsPage.Select(product => mapper.Map<ProductDto>(product)).ToList();
        
        // Create new PagedList with mapped items
        return new PagedList<ProductDto>(
            productDtos,
            productsPage.TotalCount,
            productsPage.CurrentPage,
            productsPage.PageSize
        );
    }
}
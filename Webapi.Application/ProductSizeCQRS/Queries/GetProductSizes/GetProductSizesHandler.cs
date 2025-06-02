using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.Helpers;

namespace Webapi.Application.ProductSizeCQRS.Queries.GetProductSizes;

public class GetProductSizesHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IQueryHandler<GetProductSizesQuery, PagedList<ProductSizeDto>>
{
    public async Task<PagedList<ProductSizeDto>> Handle(GetProductSizesQuery request, CancellationToken cancellationToken)
    {
        // Get product sizes with pagination
        var productSizesPage = await unitOfWork.ProductSizeRepository.GetProductSizesAsync(
            request.ProductSizeParams, cancellationToken);
            
        // Map product sizes to DTOs
        var productSizeDtos = productSizesPage.Select(size => mapper.Map<ProductSizeDto>(size)).ToList();
        
        // Create new PagedList with mapped items
        return new PagedList<ProductSizeDto>(
            productSizeDtos,
            productSizesPage.TotalCount,
            productSizesPage.CurrentPage,
            productSizesPage.PageSize
        );
    }
}
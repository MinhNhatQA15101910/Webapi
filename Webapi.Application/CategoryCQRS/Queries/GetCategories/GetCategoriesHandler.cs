using AutoMapper;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.Helpers;
using Webapi.SharedKernel.Params;

namespace Webapi.Application.CategoryCQRS.Queries.GetCategories;

public class GetCategoriesHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IQueryHandler<GetCategoriesQuery, PagedList<CategoryDto>>
{
    public async Task<PagedList<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await unitOfWork.CategoryRepository.GetCategoriesAsync(request.CategoryParams, cancellationToken);
        
        // Map the entities to DTOs while preserving pagination metadata
        return new PagedList<CategoryDto>(
            mapper.Map<List<CategoryDto>>(categories),
            categories.TotalCount,
            categories.CurrentPage,
            categories.PageSize
        );
    }
}
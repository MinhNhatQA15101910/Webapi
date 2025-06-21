using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.Helpers;
using Webapi.SharedKernel.Params;


namespace Webapi.Application.CategoryCQRS.Queries.GetCategories;

public record GetCategoriesQuery(CategoryParams CategoryParams) : IQuery<PagedList<CategoryDto>>;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.CategoryCQRS.Queries.GetCategoryById;

public record GetCategoryByIdQuery(Guid Id) : IQuery<CategoryDto>;

using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.CategoryCQRS.Commands.DeleteCategory;

public record DeleteCategoryCommand(Guid Id) : ICommand<CategoryDto>;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.CategoryCQRS.Commands.UpdateCategory;

public record UpdateCategoryCommand(Guid Id, UpdateCategoryDto CategoryDto) : ICommand<CategoryDto>;
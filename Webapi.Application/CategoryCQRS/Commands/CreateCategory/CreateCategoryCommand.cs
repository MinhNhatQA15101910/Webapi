using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.CategoryCQRS.Commands.CreateCategory;

public record CreateCategoryCommand(CreateCategoryDto CategoryDto) : ICommand<CategoryDto>;
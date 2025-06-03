using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions.Category;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.CategoryCQRS.Commands.CreateCategory;

public class CreateCategoryHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<CreateCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = httpContextAccessor.HttpContext.User.GetUserId();
            // Create new category
            var category = new Category
            {
                Name = request.CategoryDto.Name,
                Description = request.CategoryDto.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            // Add to repository
            unitOfWork.CategoryRepository.Add(category);
            await unitOfWork.CompleteAsync();
            
            // Return mapped DTO
            return mapper.Map<CategoryDto>(category);
        }
        catch (CategoryCreateException)
        {
            // Rethrow specific exceptions
            throw;
        }
        catch (Exception ex)
        {
            // Wrap other exceptions
            throw new CategoryCreateException($"An unexpected error occurred: {ex.Message}");
        }
    }
}
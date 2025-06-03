using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions.Category;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.CategoryCQRS.Commands.DeleteCategory;

public class DeleteCategoryHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<DeleteCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = httpContextAccessor.HttpContext.User.GetUserId();
            
            // Get category
            var category = await unitOfWork.CategoryRepository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new CategoryNotFoundException(request.Id);
                
            // Map to DTO for return value
            var categoryDto = mapper.Map<CategoryDto>(category);
            
            // Check if category has products
            var hasProducts = await unitOfWork.CategoryRepository.HasProductsAsync(request.Id, cancellationToken);
            if (hasProducts)
            {
                throw new CategoryInUseException(request.Id);
            }
            
            // Delete from repository
            unitOfWork.CategoryRepository.Delete(category);
            await unitOfWork.CompleteAsync();
            
            return categoryDto;
        }
        catch (CategoryNotFoundException)
        {
            // Rethrow specific exceptions
            throw;
        }
        catch (CategoryInUseException)
        {
            // Rethrow specific exceptions
            throw;
        }
        catch (Exception ex)
        {
            // Wrap other exceptions
            throw new CategoryDeleteException(request.Id, $"An unexpected error occurred: {ex.Message}");
        }
    }
}
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions;
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
        var category = await unitOfWork.CategoryRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (category == null)
        {
            throw new Exception("Category not found");
        }
        
        // Check if category has products before deleting
        var hasProducts = await unitOfWork.CategoryRepository.HasProductsAsync(request.Id, cancellationToken);
        if (hasProducts)
        {
            throw new Exception("Cannot delete category with associated products");
        }
        
        unitOfWork.CategoryRepository.Delete(category);
        return mapper.Map<CategoryDto>(category);
    }
}
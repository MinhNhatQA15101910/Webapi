using AutoMapper;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.CategoryCQRS.Commands.UpdateCategory;

public class UpdateCategoryHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<UpdateCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.CategoryRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (category == null)
        {
            throw new Exception($"Category with ID {request.Id} not found");
        }
        
        // Update category properties
        category.Name = request.CategoryDto.Name;
        category.Description = request.CategoryDto.Description;
        category.Size = request.CategoryDto.Size;
        category.Type = request.CategoryDto.Type;
        category.UpdatedAt = DateTime.UtcNow;
        
        unitOfWork.CategoryRepository.Update(category);
        return mapper.Map<CategoryDto>(category);
    }
}
using AutoMapper;
using Newtonsoft.Json;
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
        
        Console.WriteLine("Updating category: " + JsonConvert.SerializeObject(request.CategoryDto) );
        Console.WriteLine("Updating category details: ", request.CategoryDto);
        
        // Update category properties
        category.Name = request.CategoryDto.Name;
        category.Description = request.CategoryDto.Description;
        category.Size = request.CategoryDto.Size;
        category.Type = request.CategoryDto.Type;
        category.UpdatedAt = DateTime.UtcNow;
        
        
        
         unitOfWork.CategoryRepository.Update(category);
         await unitOfWork.CompleteAsync();
        return mapper.Map<CategoryDto>(category);
    }
}
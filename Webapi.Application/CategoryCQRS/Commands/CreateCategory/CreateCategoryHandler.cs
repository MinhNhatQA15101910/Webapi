using AutoMapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
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
        var userId = httpContextAccessor.HttpContext.User.GetUserId();
        
        Console.WriteLine(
            "Creating category for user with id: {0}",
            userId
            );
        
        Console.WriteLine("Creating category");
        Console.WriteLine("request: ", JsonConvert.SerializeObject(request));
        // Create a new category
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = request.CategoryDto.Name,
            Description = request.CategoryDto.Description,
            Size = request.CategoryDto.Size,
            Type = request.CategoryDto.Type,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        // Add category to database
        unitOfWork.CategoryRepository.Add(category);
        await unitOfWork.CompleteAsync();
        
        // Map to DTO and return
        return mapper.Map<CategoryDto>(category);
    }
}
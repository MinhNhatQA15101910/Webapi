using AutoMapper;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.CategoryCQRS.Queries.GetCategoryById;

public class GetCategoryByIdHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IQueryHandler<GetCategoryByIdQuery, CategoryDto>
{
    public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.CategoryRepository.GetCategoryWithDetailsAsync(request.Id, cancellationToken);
        
        if (category == null)
        {
            throw new Exception($"Category with ID {request.Id} not found");
        }
        
        return mapper.Map<CategoryDto>(category);
    }
}
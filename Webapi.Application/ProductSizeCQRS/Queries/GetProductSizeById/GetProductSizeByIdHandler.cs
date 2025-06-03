using AutoMapper;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.ProductSizeCQRS.Queries.GetProductSizeById;

public class GetProductSizeByIdHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IQueryHandler<GetProductSizeByIdQuery, ProductSizeDto>
{
    public async Task<ProductSizeDto> Handle(GetProductSizeByIdQuery request, CancellationToken cancellationToken)
    {
        var productSize = await unitOfWork.ProductSizeRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new Exception($"Product size with ID {request.Id} not found");
            
        return mapper.Map<ProductSizeDto>(productSize);
    }
}
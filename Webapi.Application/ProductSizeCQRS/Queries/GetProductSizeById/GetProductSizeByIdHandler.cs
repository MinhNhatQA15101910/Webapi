using AutoMapper;
using Webapi.Application.Common.Exceptions.ProductSize;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs.ProductSize;

namespace Webapi.Application.ProductSizeCQRS.Queries.GetProductSizeById;

public class GetProductSizeByIdHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IQueryHandler<GetProductSizeByIdQuery, ProductSizeDto>
{
    public async Task<ProductSizeDto> Handle(GetProductSizeByIdQuery request, CancellationToken cancellationToken)
    {
        var productSize = await unitOfWork.ProductSizeRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new ProductSizeNotFoundException(request.Id);
            
        return mapper.Map<ProductSizeDto>(productSize);
    }
}
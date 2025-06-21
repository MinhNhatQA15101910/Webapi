using AutoMapper;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.DTOs.ProductSize;

namespace Webapi.Application.ProductSizeCQRS.Queries.GetProductSizesByProductId;

public class GetProductSizesByProductIdHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IQueryHandler<GetProductSizesByProductIdQuery, IEnumerable<ProductSizeDto>>
{
    public async Task<IEnumerable<ProductSizeDto>> Handle(GetProductSizesByProductIdQuery request, CancellationToken cancellationToken)
    {
        var productSizes = await unitOfWork.ProductSizeRepository.GetByProductIdAsync(request.ProductId, cancellationToken);
        return mapper.Map<IEnumerable<ProductSizeDto>>(productSizes);
    }
}
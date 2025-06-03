using AutoMapper;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.ProductCQRS.Queries.GetProductPhoto;

public class GetProductPhotoHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IQueryHandler<GetProductPhotoQuery, ProductPhotoDto>
{
    public async Task<ProductPhotoDto> Handle(GetProductPhotoQuery request, CancellationToken cancellationToken)
    {
        // Check if photo exists
        var photo = await unitOfWork.ProductRepository.GetPhotoByIdAsync(
            request.PhotoId, cancellationToken);

        if (photo == null || photo.ProductId != request.ProductId)
        {
            throw new Exception($"Photo with ID {request.PhotoId} not found for product {request.ProductId}");
        }

        // Map to DTO
        return mapper.Map<ProductPhotoDto>(photo);
    }
}

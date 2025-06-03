using AutoMapper;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.DTOs.ProductPhoto;

namespace Webapi.Application.ProductCQRS.Queries.GetProductPhotos;

public class GetProductPhotosHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IQueryHandler<GetProductPhotosQuery, IEnumerable<ProductPhotoDto>>
{
    public async Task<IEnumerable<ProductPhotoDto>> Handle(GetProductPhotosQuery request, CancellationToken cancellationToken)
    {
        // Check if product exists
        var product = await unitOfWork.ProductRepository.GetByIdAsync(
            request.ProductId, cancellationToken)
            ?? throw new Exception( $"Product with id: {request.ProductId} was not found");
        
        // Get photos
        var photos = await unitOfWork.ProductRepository.GetProductPhotosAsync(
            request.ProductId, cancellationToken);
        
        // Map to DTOs
        return mapper.Map<IEnumerable<ProductPhotoDto>>(photos);
    }
}
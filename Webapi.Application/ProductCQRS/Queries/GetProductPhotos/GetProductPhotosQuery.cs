using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.ProductCQRS.Queries.GetProductPhotos;

public record GetProductPhotosQuery(Guid ProductId) : IQuery<IEnumerable<ProductPhotoDto>>;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.DTOs.ProductPhoto;

namespace Webapi.Application.ProductCQRS.Queries.GetProductPhoto;

public record GetProductPhotoQuery(Guid ProductId, Guid PhotoId) : IQuery<ProductPhotoDto>;
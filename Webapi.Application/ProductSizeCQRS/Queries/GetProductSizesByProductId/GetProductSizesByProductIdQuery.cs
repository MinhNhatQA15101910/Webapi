using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.ProductSizeCQRS.Queries.GetProductSizesByProductId;

public record GetProductSizesByProductIdQuery(Guid ProductId) : IQuery<IEnumerable<ProductSizeDto>>;
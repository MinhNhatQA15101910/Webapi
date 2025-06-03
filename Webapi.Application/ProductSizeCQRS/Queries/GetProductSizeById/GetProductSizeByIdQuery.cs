using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.ProductSizeCQRS.Queries.GetProductSizeById;

public record GetProductSizeByIdQuery(Guid Id) : IQuery<ProductSizeDto>;
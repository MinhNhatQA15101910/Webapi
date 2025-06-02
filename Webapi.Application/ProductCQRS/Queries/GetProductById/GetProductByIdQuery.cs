using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.ProductCQRS.Queries.GetProductById;

public record GetProductByIdQuery(Guid ProductId) : IQuery<ProductDto>;
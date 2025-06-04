using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.DTOs.Product;

namespace Webapi.Application.ProductCQRS.Queries.GetProductById;

public record GetProductByIdQuery(Guid ProductId) : IQuery<ProductDto>;
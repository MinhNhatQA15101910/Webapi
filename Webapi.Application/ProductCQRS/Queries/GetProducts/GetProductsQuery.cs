using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.Helpers;
using Webapi.SharedKernel.Params;

namespace Webapi.Application.ProductCQRS.Queries.GetProducts;

public record GetProductsQuery(ProductParams ProductParams) : IQuery<PagedList<ProductDto>>;
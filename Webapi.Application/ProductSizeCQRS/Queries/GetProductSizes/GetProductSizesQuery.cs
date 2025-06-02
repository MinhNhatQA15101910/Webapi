using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.Helpers;
using Webapi.SharedKernel.Params;

namespace Webapi.Application.ProductSizeCQRS.Queries.GetProductSizes;

public record GetProductSizesQuery(ProductSizeParams ProductSizeParams) : IQuery<PagedList<ProductSizeDto>>;
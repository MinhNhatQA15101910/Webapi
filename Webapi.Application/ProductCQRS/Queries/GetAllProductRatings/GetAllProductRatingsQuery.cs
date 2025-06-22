using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs.Product;
using Webapi.SharedKernel.Helpers;
using Webapi.SharedKernel.Params;

namespace Webapi.Application.ProductCQRS.Queries.GetAllProductRatings;

public record GetAllProductRatingsQuery(ProductParams ProductParams) : IQuery<IEnumerable<ProductRatingDto>>;
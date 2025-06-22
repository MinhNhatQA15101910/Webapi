using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs.Product;

namespace Webapi.Application.ProductCQRS.Queries.GetProductRating;

public record GetProductRatingQuery(Guid ProductId) : IQuery<ProductRatingDto>;
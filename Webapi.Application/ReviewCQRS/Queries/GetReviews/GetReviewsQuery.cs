using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs.Review;
using Webapi.SharedKernel.Helpers;
using Webapi.SharedKernel.Params;

namespace Webapi.Application.ReviewCQRS.Queries.GetReviews;

public record GetReviewsQuery(ReviewParams ReviewParams) : IQuery<PagedList<ReviewDto>>;
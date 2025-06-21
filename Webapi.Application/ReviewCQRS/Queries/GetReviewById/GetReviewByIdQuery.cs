using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs.Review;

namespace Webapi.Application.ReviewCQRS.Queries.GetReviewById;

public record GetReviewByIdQuery(Guid Id) : IQuery<ReviewDto>;
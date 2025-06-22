using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs.Review;

namespace Webapi.Application.ReviewCQRS.Commands.CreateReview;

public record CreateReviewCommand(CreateReviewDto ReviewDto) : ICommand<ReviewDto>;
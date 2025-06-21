using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs.Review;

namespace Webapi.Application.ReviewCQRS.Commands.UpdateReview;

public record UpdateReviewCommand(Guid Id, UpdateReviewDto ReviewDto) : ICommand<ReviewDto>;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs.Review;

namespace Webapi.Application.ReviewCQRS.Commands.DeleteReview;

public record DeleteReviewCommand(Guid Id) : ICommand<ReviewDto>;
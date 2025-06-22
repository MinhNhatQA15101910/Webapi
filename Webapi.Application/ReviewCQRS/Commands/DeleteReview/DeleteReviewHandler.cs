using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Exceptions.Review;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs.Review;

namespace Webapi.Application.ReviewCQRS.Commands.DeleteReview;

public class DeleteReviewHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<DeleteReviewCommand, ReviewDto>
{
    public async Task<ReviewDto> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = httpContextAccessor.HttpContext!.User.GetUserId();
            
            // Get review
            var review = await unitOfWork.ReviewRepository.GetReviewWithDetailsAsync(request.Id, cancellationToken)
                ?? throw new ReviewNotFoundException(request.Id);
                
            // Map to DTO for return value
            var reviewDto = mapper.Map<ReviewDto>(review);
            
            // Check if user is the owner
            if (review.OwnerId != userId)
            {
                throw new BadRequestException("You can only delete your own reviews");
            }
            
            // Delete from repository
            unitOfWork.ReviewRepository.Delete(review);
            await unitOfWork.CompleteAsync(cancellationToken);
            
            return reviewDto;
        }
        catch (ReviewNotFoundException)
        {
            // Rethrow specific exceptions
            throw;
        }
        catch (BadRequestException)
        {
            // Rethrow specific exceptions
            throw;
        }
        catch (Exception ex)
        {
            // Wrap other exceptions
            throw new ReviewDeleteException(request.Id, $"An unexpected error occurred: {ex.Message}");
        }
    }
}
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Exceptions.Review;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs.Review;

namespace Webapi.Application.ReviewCQRS.Commands.UpdateReview;

public class UpdateReviewHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<UpdateReviewCommand, ReviewDto>
{
    public async Task<ReviewDto> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = httpContextAccessor.HttpContext!.User.GetUserId();
            
            // Get review
            var review = await unitOfWork.ReviewRepository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new ReviewNotFoundException(request.Id);
                
            // Check if user is the owner
            if (review.OwnerId != userId)
            {
                throw new BadRequestException("You can only update your own reviews");
            }
            
            // Update review properties
            review.Title = request.ReviewDto.Title;
            review.Content = request.ReviewDto.Content;
            review.Rating = request.ReviewDto.Rating;
            review.UpdatedAt = DateTime.UtcNow;
            
            // Update in repository
            unitOfWork.ReviewRepository.Update(review);
            await unitOfWork.CompleteAsync(cancellationToken);
            
            // Get the updated review with all related details
            var updatedReview = await unitOfWork.ReviewRepository.GetReviewWithDetailsAsync(review.Id, cancellationToken)
                ?? throw new ReviewNotFoundException(review.Id);
                
            // Return mapped DTO
            return mapper.Map<ReviewDto>(updatedReview);
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
            throw new ReviewUpdateException(request.Id, $"An unexpected error occurred: {ex.Message}");
        }
    }
}
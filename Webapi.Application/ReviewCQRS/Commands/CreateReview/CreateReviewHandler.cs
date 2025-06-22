using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Exceptions.Product;
using Webapi.Application.Common.Exceptions.Review;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs.Review;

namespace Webapi.Application.ReviewCQRS.Commands.CreateReview;

public class CreateReviewHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<CreateReviewCommand, ReviewDto>
{
    public async Task<ReviewDto> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = httpContextAccessor.HttpContext!.User.GetUserId();
            
            // Verify product exists
            var product = await unitOfWork.ProductRepository.GetByIdAsync(request.ReviewDto.ProductId, cancellationToken)
                ?? throw new ProductNotFoundException(request.ReviewDto.ProductId);
                
            // Create new review
            var review = new Review
            {
                Title = request.ReviewDto.Title,
                Content = request.ReviewDto.Content,
                Rating = request.ReviewDto.Rating,
                ProductId = request.ReviewDto.ProductId,
                OwnerId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            // Add to repository
            unitOfWork.ReviewRepository.Add(review);
            await unitOfWork.CompleteAsync(cancellationToken);
            
            // Get the review with all related details for return
            var createdReview = await unitOfWork.ReviewRepository.GetReviewWithDetailsAsync(review.Id, cancellationToken)
                ?? throw new ReviewNotFoundException(review.Id);
                
            // Return mapped DTO
            return mapper.Map<ReviewDto>(createdReview);
        }
        catch (ProductNotFoundException)
        {
            // Rethrow specific exceptions
            throw;
        }
        catch (Exception ex)
        {
            // Wrap other exceptions
            throw new ReviewCreateException($"An unexpected error occurred: {ex.Message}");
        }
    }
}
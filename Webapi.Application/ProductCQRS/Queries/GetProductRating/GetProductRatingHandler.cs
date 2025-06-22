using AutoMapper;
using Webapi.Application.Common.Exceptions.Product;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs.Product;

namespace Webapi.Application.ProductCQRS.Queries.GetProductRating;

public class GetProductRatingHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IQueryHandler<GetProductRatingQuery, ProductRatingDto>
{
    public async Task<ProductRatingDto> Handle(GetProductRatingQuery request, CancellationToken cancellationToken)
    {
        // Verify product exists
        var product = await unitOfWork.ProductRepository.GetByIdAsync(request.ProductId, cancellationToken)
            ?? throw new ProductNotFoundException(request.ProductId);

        // Get all reviews for the product
        var reviews = await unitOfWork.ReviewRepository.GetReviewsByProductIdAsync(request.ProductId, cancellationToken);
        
        // Calculate average rating
        var reviewList = reviews.ToList();
        int reviewCount = reviewList.Count;
        float averageRating = 0;
        
        // Initialize star counts
        int oneStarCount = 0;
        int twoStarCount = 0;
        int threeStarCount = 0;
        int fourStarCount = 0;
        int fiveStarCount = 0;
        
        if (reviewCount > 0)
        {
            averageRating = reviewList.Average(r => r.Rating);
            
            // Count reviews by star rating
            foreach (var review in reviewList)
            {
                switch (Math.Round(review.Rating))
                {
                    case 1:
                        oneStarCount++;
                        break;
                    case 2:
                        twoStarCount++;
                        break;
                    case 3:
                        threeStarCount++;
                        break;
                    case 4:
                        fourStarCount++;
                        break;
                    case 5:
                        fiveStarCount++;
                        break;
                }
            }
        }
        
        // Return rating DTO
        return new ProductRatingDto
        {
            ProductId = product.Id,
            ProductName = product.Name,
            AverageRating = averageRating,
            ReviewCount = reviewCount,
            OneStarCount = oneStarCount,
            TwoStarCount = twoStarCount,
            ThreeStarCount = threeStarCount,
            FourStarCount = fourStarCount,
            FiveStarCount = fiveStarCount
        };
    }
}
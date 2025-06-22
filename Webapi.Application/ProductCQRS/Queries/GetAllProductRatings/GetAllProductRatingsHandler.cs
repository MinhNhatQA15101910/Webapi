using AutoMapper;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs.Product;

namespace Webapi.Application.ProductCQRS.Queries.GetAllProductRatings;

public class GetAllProductRatingsHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IQueryHandler<GetAllProductRatingsQuery, IEnumerable<ProductRatingDto>>
{
    public async Task<IEnumerable<ProductRatingDto>> Handle(GetAllProductRatingsQuery request, CancellationToken cancellationToken)
    {
        // Get paged products
        var products = await unitOfWork.ProductRepository.GetProductsAsync(request.ProductParams, cancellationToken);
        
        // Create rating DTOs
        var productRatings = new List<ProductRatingDto>();
        
        foreach (var product in products)
        {
            // Get reviews for this product
            var reviews = await unitOfWork.ReviewRepository.GetReviewsByProductIdAsync(product.Id, cancellationToken);
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
            
            // Add to result list
            productRatings.Add(new ProductRatingDto
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
            });
        }
        
        // Create paged list with same pagination metadata as the products
        return mapper.Map<IEnumerable<ProductRatingDto>>(productRatings);
    }
}
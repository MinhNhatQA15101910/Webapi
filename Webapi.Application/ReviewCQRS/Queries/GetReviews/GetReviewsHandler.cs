using AutoMapper;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs.Review;
using Webapi.SharedKernel.Helpers;

namespace Webapi.Application.ReviewCQRS.Queries.GetReviews;

public class GetReviewsHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IQueryHandler<GetReviewsQuery, PagedList<ReviewDto>>
{
    public async Task<PagedList<ReviewDto>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
    {
        var reviews = await unitOfWork.ReviewRepository.GetReviewsAsync(request.ReviewParams, cancellationToken);
        
        // Map the entities to DTOs while preserving pagination metadata
        return new PagedList<ReviewDto>(
            mapper.Map<List<ReviewDto>>(reviews),
            reviews.TotalCount,
            reviews.CurrentPage,
            reviews.PageSize
        );
    }
}
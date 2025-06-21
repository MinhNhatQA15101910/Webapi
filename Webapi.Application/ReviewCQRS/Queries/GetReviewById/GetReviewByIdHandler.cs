using AutoMapper;
using Webapi.Application.Common.Exceptions.Review;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs.Review;

namespace Webapi.Application.ReviewCQRS.Queries.GetReviewById;

public class GetReviewByIdHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IQueryHandler<GetReviewByIdQuery, ReviewDto>
{
    public async Task<ReviewDto> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
    {
        var review = await unitOfWork.ReviewRepository.GetReviewWithDetailsAsync(request.Id, cancellationToken)
            ?? throw new ReviewNotFoundException(request.Id);

        return mapper.Map<ReviewDto>(review);
    }
}
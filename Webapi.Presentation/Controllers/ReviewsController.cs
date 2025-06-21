using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webapi.Application.ReviewCQRS.Commands.CreateReview;
using Webapi.Application.ReviewCQRS.Commands.DeleteReview;
using Webapi.Application.ReviewCQRS.Commands.UpdateReview;
using Webapi.Application.ReviewCQRS.Queries.GetReviewById;
using Webapi.Application.ReviewCQRS.Queries.GetReviews;
using Webapi.Presentation.Extensions;
using Webapi.SharedKernel.DTOs.Review;
using Webapi.SharedKernel.Params;

namespace Webapi.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviews([FromQuery] ReviewParams reviewParams)
    {
        var reviews = await mediator.Send(new GetReviewsQuery(reviewParams));
        
        Response.AddPaginationHeader(reviews);
        
        return Ok(reviews);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReviewDto>> GetReview(Guid id)
    {
        var review = await mediator.Send(new GetReviewByIdQuery(id));
        return Ok(review);
    }

    [HttpPost]
    public async Task<ActionResult<ReviewDto>> CreateReview(CreateReviewDto createReviewDto)
    {
        var review = await mediator.Send(new CreateReviewCommand(createReviewDto));
        
        return CreatedAtAction(
            nameof(GetReview),
            new { id = review.Id },
            review
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ReviewDto>> UpdateReview(Guid id, UpdateReviewDto updateReviewDto)
    {
        var updatedReview = await mediator.Send(new UpdateReviewCommand(id, updateReviewDto));
        return Ok(updatedReview);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ReviewDto>> DeleteReview(Guid id)
    {
        var deletedReview = await mediator.Send(new DeleteReviewCommand(id));
        return Ok(deletedReview);
    }
}
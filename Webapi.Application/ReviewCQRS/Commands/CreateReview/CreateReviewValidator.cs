using FluentValidation;

namespace Webapi.Application.ReviewCQRS.Commands.CreateReview;

public class CreateReviewValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewValidator()
    {
        RuleFor(x => x.ReviewDto.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters");
            
        RuleFor(x => x.ReviewDto.Content)
            .NotEmpty().WithMessage("Content is required")
            .MaximumLength(1000).WithMessage("Content must not exceed 1000 characters");
            
        RuleFor(x => x.ReviewDto.Rating)
            .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5");
            
        RuleFor(x => x.ReviewDto.ProductId)
            .NotEmpty().WithMessage("Product ID is required");
    }
}
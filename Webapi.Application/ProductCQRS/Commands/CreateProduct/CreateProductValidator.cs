using FluentValidation;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.ProductCQRS.Commands.CreateProduct;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateProductValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        
        RuleFor(x => x.ProductDto.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");
            
        RuleFor(x => x.ProductDto.Description)
            .NotEmpty().WithMessage("Description is required");
            
        RuleFor(x => x.ProductDto.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");
            
        RuleFor(x => x.ProductDto.InStock)
            .GreaterThanOrEqualTo(0).WithMessage("In stock must be greater than or equal to 0");
            
        RuleFor(x => x.ProductDto.CategoryIds)
            .NotEmpty().WithMessage("At least one category is required");
            
    }
}
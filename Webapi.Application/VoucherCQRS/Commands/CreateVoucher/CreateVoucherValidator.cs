using FluentValidation;

namespace Webapi.Application.VoucherCQRS.Commands.CreateVoucher;

public class CreateVoucherValidator : AbstractValidator<CreateVoucherCommand>
{
    public CreateVoucherValidator()
    {
        RuleFor(x => x.VoucherDto.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");
            
        RuleFor(x => x.VoucherDto.Value)
            .GreaterThan(0).WithMessage("Value must be greater than 0");
            
        RuleFor(x => x.VoucherDto.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}
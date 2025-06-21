using FluentValidation;

namespace Webapi.Application.VoucherCQRS.Commands.ImportVouchers;

public class ImportVouchersValidator : AbstractValidator<ImportVouchersCommand>
{
    public ImportVouchersValidator()
    {
        RuleFor(x => x.ImportDto.File)
            .NotNull().WithMessage("File is required");
            
        RuleFor(x => x.ImportDto.ImportFormat)
            .NotEmpty().WithMessage("Import format is required")
            .Must(BeValidFormat).WithMessage("Import format must be 'json' or 'excel'");
    }
    
    private bool BeValidFormat(string format)
    {
        return format.ToLower() is "json" or "excel";
    }
}
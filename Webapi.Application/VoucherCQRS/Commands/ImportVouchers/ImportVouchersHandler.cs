using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Interfaces.Factories;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Application.Common.Interfaces.Services;
using Webapi.Domain.Interfaces;


namespace Webapi.Application.VoucherCQRS.Commands.ImportVouchers;

public class ImportVouchersHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IVoucherImportFactory importFactory
) : ICommandHandler<ImportVouchersCommand, int>
{
    public async Task<int> Handle(ImportVouchersCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.ImportDto.File == null || request.ImportDto.File.Length == 0)
            {
                throw new BadRequestException("No file was uploaded");
            }
            
            // Get the appropriate importer based on format
            var importer = importFactory.CreateImporter(request.ImportDto.ImportFormat);
            
            // Import vouchers
            using var stream = request.ImportDto.File.OpenReadStream();
            var vouchers = await importer.ImportVouchersAsync(stream, request.ImportDto.File.FileName);
            
            if (vouchers == null || !vouchers.Any())
            {
                return 0;
            }
            
            // Save to database
            foreach (var voucher in vouchers)
            {
                unitOfWork.VoucherRepository.Add(voucher);
            }
            
            await unitOfWork.CompleteAsync(cancellationToken);
            
            return vouchers.Count();
        }
        catch (Exception ex) when (
            ex is not BadRequestException && 
            ex is not NotSupportedException)
        {
            throw new BadRequestException($"Failed to import vouchers: {ex.Message}");
        }
    }
}
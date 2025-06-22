using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Interfaces.Factories;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Factories;
using Webapi.Domain.Interfaces;


namespace Webapi.Application.VoucherCQRS.Commands.ImportVouchers;

public class ImportVouchersHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IVoucherImportFactory importFactory,
    VoucherFactory voucherFactory 
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
            var voucherData = await importer.ImportVouchersAsync(stream, request.ImportDto.File.FileName);
            
            if (voucherData == null || !voucherData.Any())
            {
                return 0;
            }
            
            // Count how many vouchers we're adding
            int voucherCount = 0;
            
            // Use the factory to create vouchers with flyweight pattern
            foreach (var data in voucherData)
            {
                // Use flyweight pattern to get voucher type
                var voucherType = voucherFactory.GetVoucherType(
                    data.Type.Name,
                    data.Type.Value,
                    data.ExpiredAt
                );
                
                // Use prototype pattern to create voucher with items
                var voucher = voucherFactory.CreateVoucher(voucherType, data.Quantity, data.ExpiredAt);
                
                // Add to repository
                unitOfWork.VoucherRepository.Add(voucher);
                voucherCount++;
            }
            
            await unitOfWork.CompleteAsync(cancellationToken);
            
            return voucherCount;
        }
        catch (Exception ex) when (
            ex is not BadRequestException && 
            ex is not NotSupportedException)
        {
            throw new BadRequestException($"Failed to import vouchers: {ex.Message}");
        }
    }
}
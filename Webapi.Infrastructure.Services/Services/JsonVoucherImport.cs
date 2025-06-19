using System.Text.Json;
using Webapi.Application.Common.Interfaces.Services;
using Webapi.Domain.Entities;

namespace Webapi.Infrastructure.Services.Services;

public class JsonVoucherImport : IVoucherImport
{
    public async Task<IEnumerable<Voucher>> ImportVouchersAsync(Stream fileStream, string fileName)
    {
        if (fileStream == null)
        {
            throw new ArgumentNullException(nameof(fileStream));
        }
        
        using var reader = new StreamReader(fileStream);
        var jsonData = await reader.ReadToEndAsync();
        
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        
        var vouchers = JsonSerializer.Deserialize<List<Voucher>>(jsonData, options);
        
        if (vouchers == null)
        {
            return Enumerable.Empty<Voucher>();
        }
        
        // Ensure all vouchers have proper UTC dates and IDs
        foreach (var voucher in vouchers)
        {
            if (voucher.Id == Guid.Empty)
            {
                voucher.Id = Guid.NewGuid();
            }
            
            voucher.CreatedAt = DateTime.UtcNow;
            voucher.UpdatedAt = DateTime.UtcNow;
            
            if (voucher.ExpiredAt == default)
            {
                voucher.ExpiredAt = DateTime.UtcNow.AddMonths(3);
            }
        }
        
        return vouchers;
    }
}
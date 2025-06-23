using System.Text.Json;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Interfaces.Services;
using Webapi.Domain.Entities;
using Webapi.Domain.Factories;

namespace Webapi.Infrastructure.Services.Services;

public class JsonVoucherImport : IVoucherImport
{
    private readonly VoucherFactory _voucherFactory;

    public JsonVoucherImport(VoucherFactory voucherFactory)
    {
        _voucherFactory = voucherFactory;
    }

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
        
        try
        {
            // Create a structure that matches our JSON
            var voucherData = JsonSerializer.Deserialize<List<VoucherImportData>>(jsonData, options);
            
            if (voucherData == null)
            {
                throw new BadRequestException("Could not parse vouchers from JSON");
            }
            
            var vouchers = new List<Voucher>();
            
            foreach (var data in voucherData)
            {
                var expireDate = data.ExpiredAt ?? DateTime.UtcNow.AddMonths(3);
                
                // Use the factory to create vouchers using the patterns
                var voucherType = _voucherFactory.GetVoucherType(
                    data.Type.Name, 
                    data.Type.Value, 
                    expireDate
                );
                
                var voucher = _voucherFactory.CreateVoucher(
                    voucherType, 
                    data.Quantity, 
                    expireDate
                );
                
                vouchers.Add(voucher);
            }
            
            return vouchers;
        }
        catch (JsonException ex)
        {
            throw new BadRequestException($"JSON parsing error: {ex.Message}");
        }
    }
}

internal class VoucherTypeData
{
    public string Name { get; set; } = string.Empty;
    public double Value { get; set; }
}

internal class VoucherImportData
{
    public VoucherTypeData Type { get; set; } = new();
    public int Quantity { get; set; }
    public DateTime? ExpiredAt { get; set; }
}
using System.Data;
using ExcelDataReader;
using Webapi.Application.Common.Interfaces.Services;
using Webapi.Domain.Entities;
using Webapi.Domain.Factories;
using Webapi.SharedKernel.DTOs.Voucher;

namespace Webapi.Infrastructure.Services.Services;

public class XlsVoucherImport : IVoucherImport
{
    private readonly VoucherFactory _voucherFactory;

    public XlsVoucherImport(VoucherFactory voucherFactory)
    {
        _voucherFactory = voucherFactory;
    }

    public async Task<IEnumerable<Voucher>> ImportVouchersAsync(Stream fileStream, string fileName)
    {
        if (fileStream == null)
        {
            throw new ArgumentNullException(nameof(fileStream));
        }
        
        // This makes the code work with Excel on .NET Core
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        
        var importData = new List<Voucher>();
        
        using (var reader = ExcelReaderFactory.CreateReader(fileStream))
        {
            var result = reader.AsDataSet(new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration
                {
                    UseHeaderRow = true
                }
            });
            
            DataTable dataTable = result.Tables[0];
            
            foreach (DataRow row in dataTable.Rows)
            {
                DateTime expireDate = DateTime.UtcNow.AddMonths(3);
                if (DateTime.TryParse(row["ExpiredAt"]?.ToString(), out var parsedDate))
                {
                    expireDate = parsedDate;
                }
                
                string name = row["Name"]?.ToString() ?? string.Empty;
                double value = Convert.ToDouble(row["Value"] ?? 0);
                int quantity = Convert.ToInt32(row["Quantity"] ?? 0);
                
                // Use the factory to create vouchers using the patterns
                var voucherType = _voucherFactory.GetVoucherType(name, value, expireDate);
                var voucher = _voucherFactory.CreateVoucher(voucherType, quantity, expireDate);
                
                importData.Add(voucher);
            }
        }
        
        return await Task.FromResult(importData);
    }
}
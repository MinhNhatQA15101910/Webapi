using System.Data;
using ExcelDataReader;
using Webapi.Domain.Entities;

namespace Webapi.Infrastructure.Services.Services;

public class XlsVoucherImport
{
    public IEnumerable<Voucher> ImportFromExcel(Stream fileStream, string fileName)
    {
        if (fileStream == null)
        {
            throw new ArgumentNullException(nameof(fileStream));
        }
        
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        
        var vouchers = new List<Voucher>();
        
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
                var voucher = new Voucher
                {
                    Id = Guid.NewGuid(),
                    Name = row["Name"].ToString() ?? string.Empty,
                    Value = Convert.ToDouble(row["Value"]),
                    Quantity = Convert.ToInt32(row["Quantity"]),
                    ExpiredAt = DateTime.Parse(row["ExpiredAt"].ToString() ?? DateTime.UtcNow.AddMonths(3).ToString()),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                
                vouchers.Add(voucher);
            }
        }
        
        return vouchers;
    }
}
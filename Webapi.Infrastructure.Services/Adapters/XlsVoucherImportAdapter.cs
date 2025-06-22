using Webapi.Application.Common.Interfaces.Services;
using Webapi.Domain.Entities;
using Webapi.Infrastructure.Services.Services;
using Webapi.SharedKernel.DTOs.Voucher;

namespace Webapi.Infrastructure.Services.Adapters;

public class XlsVoucherImportAdapter : IVoucherImport
{
    private readonly XlsVoucherImport _xlsImporter;
    
    public XlsVoucherImportAdapter(XlsVoucherImport xlsImporter)
    {
        _xlsImporter = xlsImporter;
    }
    
    public async Task<IEnumerable<Voucher>> ImportVouchersAsync(Stream fileStream, string fileName)
    {
        // Since the original XLS importer is synchronous, we wrap it in a Task
        return await _xlsImporter.ImportVouchersAsync(fileStream, fileName);
    }
}   
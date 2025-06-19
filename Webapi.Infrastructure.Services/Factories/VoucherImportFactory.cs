using Microsoft.Extensions.DependencyInjection;
using Webapi.Application.Common.Interfaces.Factories;
using Webapi.Application.Common.Interfaces.Services;
using Webapi.Infrastructure.Services.Adapters;
using Webapi.Infrastructure.Services.Services;

namespace Webapi.Infrastructure.Services.Factories;

public class VoucherImportFactory : IVoucherImportFactory
{
    private readonly IServiceProvider _serviceProvider;
    
    public VoucherImportFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public IVoucherImport CreateImporter(string format)
    {
        return format.ToLowerInvariant() switch
        {
            "json" => _serviceProvider.GetRequiredService<JsonVoucherImport>(),
            "excel" or "xls" or "xlsx" => _serviceProvider.GetRequiredService<XlsVoucherImportAdapter>(),
            _ => throw new NotSupportedException($"Import format {format} is not supported.")
        };
    }
}
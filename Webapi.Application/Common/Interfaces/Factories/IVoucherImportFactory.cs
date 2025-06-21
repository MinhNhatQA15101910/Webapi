using Webapi.Application.Common.Interfaces.Services;

namespace Webapi.Application.Common.Interfaces.Factories;

public interface IVoucherImportFactory
{
    IVoucherImport CreateImporter(string format);
}
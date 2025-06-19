using Webapi.Domain.Entities;
using System.IO;

namespace Webapi.Application.Common.Interfaces.Services;

public interface IVoucherImport
{
    Task<IEnumerable<Voucher>> ImportVouchersAsync(Stream fileStream, string fileName);
}
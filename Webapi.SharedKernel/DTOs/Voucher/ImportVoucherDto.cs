using Microsoft.AspNetCore.Http;

namespace Webapi.SharedKernel.DTOs.Voucher;

public class ImportVoucherDto
{
    public IFormFile File { get; set; }
    public string ImportFormat { get; set; } = string.Empty;  // "json" or "excel"
}
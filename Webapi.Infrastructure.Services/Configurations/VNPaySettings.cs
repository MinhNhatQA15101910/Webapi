using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webapi.Infrastructure.Services.Configurations;

public class VNPaySettings
{
    public required string TmnCode { get; set; }
    public required string HashSecret { get; set; }
    public required string BaseUrl { get; set; }
    public required string ReturnUrl { get; set; }
    public required string Command { get; set; }
    public required string CurrCode { get; set; }
    public required string Version { get; set; }
    public required string Locale { get; set; }
    public required string RemoteIpAddress { get; set; }
}

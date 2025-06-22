using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webapi.Infrastructure.Services.Configurations;

public class MomoSettings
{
    public required string BaseUrl { get; set; } 
    public required string PartnerCode { get; set; }
    public required string RequestId { get; set; }
    public required string AccessKey { get; set; }
    public required string SecretKey { get; set; } 
    public required string RedirectUrl { get; set; } 
    public required string IpnUrl { get; set; }
    public required string Lang { get; set; }
    public required string RequestType { get; set; } 
    public required string ExtraData { get; set; } 
    public required string StoreId { get; set; }
}

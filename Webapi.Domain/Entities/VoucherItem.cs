namespace Webapi.Domain.Entities;

public class VoucherItem
{
    public Guid Id { get; set; }
    public Guid VoucherId { get; set; }
    public bool Status { get; set; } = true;
    
    
    // Clone method for Prototype pattern
    public VoucherItem Clone()
    {
        return new VoucherItem
        {
            Id = Guid.NewGuid(), 
            VoucherId = this.VoucherId,
            Status = this.Status,
        };
    }
}
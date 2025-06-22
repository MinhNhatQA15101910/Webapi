using Microsoft.EntityFrameworkCore;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.Infrastructure.Persistence.Data;

namespace Webapi.Infrastructure.Persistence.Repositories;

public class VoucherItemRepository(AppDbContext context) : IVoucherItemRepository
{
    private readonly AppDbContext _context = context;
    
    public async Task<VoucherItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.VoucherItems
            .FirstOrDefaultAsync(vi => vi.Id == id, cancellationToken);
    }
    
    public async Task<IEnumerable<VoucherItem>> GetByVoucherIdAsync(Guid voucherId, CancellationToken cancellationToken = default)
    {
        return await _context.VoucherItems
            .Where(vi => vi.VoucherId == voucherId)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<VoucherItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.VoucherItems
            .ToListAsync(cancellationToken);
    }
    
    public void Add(VoucherItem voucherItem)
    {
        _context.VoucherItems.Add(voucherItem);
    }
    
    public void AddRange(IEnumerable<VoucherItem> voucherItems)
    {
        _context.VoucherItems.AddRange(voucherItems);
    }
    
    public void Update(VoucherItem voucherItem)
    {
        _context.VoucherItems.Update(voucherItem);
    }
    
    public void Remove(VoucherItem voucherItem)
    {
        _context.VoucherItems.Remove(voucherItem);
    }
    
    public void RemoveRange(IEnumerable<VoucherItem> voucherItems)
    {
        _context.VoucherItems.RemoveRange(voucherItems);
    }
    
    public async Task<int> CountAvailableByVoucherIdAsync(Guid voucherId, CancellationToken cancellationToken = default)
    {
        return await _context.VoucherItems
            .Where(vi => vi.VoucherId == voucherId && vi.Status == true)
            .CountAsync(cancellationToken);
    }
}
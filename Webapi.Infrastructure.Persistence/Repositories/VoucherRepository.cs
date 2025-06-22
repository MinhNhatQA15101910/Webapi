using Microsoft.EntityFrameworkCore;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.Infrastructure.Persistence.Data;

namespace Webapi.Infrastructure.Persistence.Repositories;

public class VoucherRepository(AppDbContext context) : IVoucherRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<Voucher>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Vouchers
            .Include(v => v.Type)
            .OrderByDescending(v => v.CreatedAt)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<Voucher>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Vouchers
            .Include(v => v.Type)
            .Include(v => v.Orders)
            .OrderByDescending(v => v.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Voucher?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Vouchers
            .Include(v => v.Type)
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }
    
    public async Task<Voucher?> GetVoucherWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Vouchers
            .Include(v => v.Type)
            .Include(v => v.Items)
            .Include(v => v.Orders)
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public void Add(Voucher voucher)
    {
        _context.Vouchers.Add(voucher);
    }

    public void Update(Voucher voucher)
    {
        _context.Vouchers.Update(voucher);
    }

    public void Remove(Voucher voucher)
    {
        // First remove related items if they exist
        if (voucher.Items != null && voucher.Items.Any())
        {
            _context.VoucherItems.RemoveRange(voucher.Items);
        }
        
        _context.Vouchers.Remove(voucher);
    }
    
    public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Vouchers.AnyAsync(cancellationToken);
    }
}
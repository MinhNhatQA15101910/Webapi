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
            .OrderByDescending(v => v.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Voucher?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Vouchers
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public async Task<Voucher?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Vouchers
            .FirstOrDefaultAsync(v => v.Name == name, cancellationToken);
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
        _context.Vouchers.Remove(voucher);
    }
}
using Microsoft.EntityFrameworkCore;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;

namespace Webapi.Infrastructure.Persistence.Repositories;

public class VoucherRepository(AppDbContext context) : IVoucherRepository
{
    public async Task<IEnumerable<Voucher>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Vouchers
            .OrderByDescending(v => v.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Voucher?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Vouchers
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public async Task<Voucher?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await context.Vouchers
            .FirstOrDefaultAsync(v => v.Name == name, cancellationToken);
    }

    public void Add(Voucher voucher)
    {
        context.Vouchers.Add(voucher);
    }

    public void Update(Voucher voucher)
    {
        context.Vouchers.Update(voucher);
    }

    public void Remove(Voucher voucher)
    {
        context.Vouchers.Remove(voucher);
    }

    public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return await context.Vouchers
            .AnyAsync(cancellationToken);
    }
}
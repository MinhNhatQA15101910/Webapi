using Webapi.Domain.Entities;

namespace Webapi.Domain.Interfaces;

public interface IVoucherRepository
{
    Task<Voucher?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Voucher?> GetVoucherWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Voucher>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Voucher>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
    void Add(Voucher voucher);
    void Update(Voucher voucher);
    void Remove(Voucher voucher);
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);
}

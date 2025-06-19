using Webapi.Domain.Entities;

namespace Webapi.Domain.Interfaces;

public interface IVoucherRepository
{
    Task<IEnumerable<Voucher>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Voucher?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Voucher?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    void Add(Voucher voucher);
    void Update(Voucher voucher);
    void Remove(Voucher voucher);
}
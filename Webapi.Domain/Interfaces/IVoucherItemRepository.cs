using Webapi.Domain.Entities;

namespace Webapi.Domain.Interfaces;

public interface IVoucherItemRepository
{
    Task<VoucherItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<VoucherItem>> GetByVoucherIdAsync(Guid voucherId, CancellationToken cancellationToken = default);
    Task<IEnumerable<VoucherItem>> GetAllAsync(CancellationToken cancellationToken = default);
    void Add(VoucherItem voucherItem);
    void AddRange(IEnumerable<VoucherItem> voucherItems);
    void Update(VoucherItem voucherItem);
    void Remove(VoucherItem voucherItem);
    void RemoveRange(IEnumerable<VoucherItem> voucherItems);
}
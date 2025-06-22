using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.Infrastructure.Persistence.Repositories;

namespace Webapi.Infrastructure.Persistence.Proxies;

public class VoucherProxy(
    VoucherRepository voucherRepository,
    ICacheService cacheService
) : IVoucherRepository
{
    public void Add(Voucher voucher)
    {
        voucherRepository.Add(voucher);

        UpdateCacheForVoucherAdded();
    }

    public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return await voucherRepository.AnyAsync(cancellationToken);
    }

    public async Task<IEnumerable<Voucher>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var cacheKey = "Vouchers_?";

        if (!cacheService.TryGetValue(cacheKey, out IEnumerable<Voucher>? vouchers))
        {
            vouchers = await voucherRepository.GetAllAsync(cancellationToken);

            cacheService.Set(cacheKey, vouchers);
        }

        return vouchers ?? [];
    }

    public async Task<Voucher?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"Vouchers_/{id}";

        if (!cacheService.TryGetValue(cacheKey, out Voucher? voucher))
        {
            voucher = await voucherRepository.GetByIdAsync(id, cancellationToken);

            cacheService.Set(cacheKey, voucher);
        }

        return voucher;
    }

    public async Task<Voucher?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"Vouchers_/name={name}";

        if (!cacheService.TryGetValue(cacheKey, out Voucher? voucher))
        {
            voucher = await voucherRepository.GetByNameAsync(name, cancellationToken);

            cacheService.Set(cacheKey, voucher);
        }

        return voucher;
    }

    public void Remove(Voucher voucher)
    {
        voucherRepository.Remove(voucher);

        UpdateCacheForVoucherRemoved(voucher);
    }

    public void Update(Voucher voucher)
    {
        voucherRepository.Update(voucher);

        UpdateCacheForVoucherUpdated(voucher);
    }

    private void UpdateCacheForVoucherAdded()
    {
        var cacheKey = "Vouchers_?";
        cacheService.RemoveKeysStartingWith(cacheKey);
    }

    private void UpdateCacheForVoucherUpdated(Voucher voucher)
    {
        var cacheKeyForList = "Vouchers_?";
        cacheService.RemoveKeysStartingWith(cacheKeyForList);

        var cacheKeyForSingleWithId = $"Vouchers_/{voucher.Id}";
        cacheService.Set(cacheKeyForSingleWithId, voucher);

        var cacheKeyForSingleWithName = $"Vouchers_/name={voucher.Name}";
        cacheService.Set(cacheKeyForSingleWithName, voucher);
    }

    private void UpdateCacheForVoucherRemoved(Voucher voucher)
    {
        var cacheKeyForList = "Vouchers_?";
        cacheService.RemoveKeysStartingWith(cacheKeyForList);

        var cacheKeyForSingleWithId = $"Vouchers_/{voucher.Id}";
        cacheService.Remove(cacheKeyForSingleWithId);

        var cacheKeyForSingleWithName = $"Vouchers_/name={voucher.Name}";
        cacheService.Remove(cacheKeyForSingleWithName);
    }
}

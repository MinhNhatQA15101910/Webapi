using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Webapi.Domain.Interfaces;
using Webapi.Infrastructure.Services.Configurations;

namespace Webapi.Infrastructure.Services.Services;

public class CacheService(
    IOptions<CacheSettings> config,
    IMemoryCache cache
) : ICacheService
{
    private readonly ConcurrentDictionary<string, bool> _cacheKeys = new();
    private readonly int _cacheAbsoluteDurationMinutes = config.Value.CacheAbsoluteDurationMinutes;
    private readonly int _cacheSlidingDurationMinutes = config.Value.CacheSlidingDurationMinutes;

    public void Set<T>(string key, T value)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_cacheAbsoluteDurationMinutes),
            SlidingExpiration = TimeSpan.FromMinutes(_cacheSlidingDurationMinutes),
        };

        cache.Set(key, value, cacheEntryOptions);
        _cacheKeys.TryAdd(key, true);
    }

    public bool TryGetValue<T>(string key, out T? value)
    {
        if (cache.TryGetValue(key, out value))
        {
            return true;
        }

        _cacheKeys.TryRemove(key, out _);
        value = default;
        return false;
    }

    public void Remove(string key)
    {
        cache.Remove(key);
        _cacheKeys.TryRemove(key, out _);
    }

    public List<string> GetAllKeys()
    {
        return [.. _cacheKeys.Keys];
    }

    public void Clear()
    {
        foreach (var key in _cacheKeys.Keys)
        {
            cache.Remove(key);
        }
        _cacheKeys.Clear();
    }

    public void RemoveKeysStartingWith(string keyPart)
    {
        var keysToRemove = _cacheKeys.Keys.Where(k => k.StartsWith(keyPart)).ToList();
        foreach (var key in keysToRemove)
        {
            cache.Remove(key);
            _cacheKeys.TryRemove(key, out _);
        }
    }
}

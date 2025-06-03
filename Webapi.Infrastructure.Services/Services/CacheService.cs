using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;
using Webapi.Domain.Interfaces;

namespace Webapi.Infrastructure.Services.Services;

public class CacheService(IMemoryCache cache) : ICacheService
{
    private readonly ConcurrentDictionary<string, bool> _cacheKeys = new();

    public void Set<T>(string key, T value, MemoryCacheEntryOptions options)
    {
        cache.Set(key, value, options);
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
}

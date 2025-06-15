namespace Webapi.Domain.Interfaces;

public interface ICacheService
{
    // Adds a cache entry and tracks its key in our ConcurrentDictionary.
    // options can define expiration strategies, priority, etc.
    void Set<T>(string key, T value);

    // Attempts to retrieve a cache entry.
    // If the key exists in the IMemoryCache, returns true along with the value.
    // Otherwise, removes it from our dictionary.
    bool TryGetValue<T>(string key, out T? value);

    // Removes a cache entry from both IMemoryCache and our dictionary.
    void Remove(string key);

    void RemoveKeysStartingWith(string keyPart);

    // Returns all currently known (tracked) cache keys.
    // Note: This might include keys that recently expired, so you may want to
    // re-check each key in IMemoryCache if you want only actively stored ones.
    List<string> GetAllKeys();

    // Clears all cache entries from IMemoryCache and resets our dictionary.
    void Clear();
}

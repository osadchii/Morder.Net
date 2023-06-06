using Infrastructure.Cache.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Cache;

public class EntityIdCacheService<T> : IEntityIdCacheService<T>
{
    private readonly string _cacheKeyBase = typeof(T).FullName ?? throw new InvalidOperationException();
    private readonly IMemoryCache _cache;

    public EntityIdCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public bool TryGetValue(string key, out int value)
    {
        var fullKey = GetFullKey(key);
        return _cache.TryGetValue(fullKey, out value);
    }

    public void Set(string key, int value)
    {
        var fullKey = GetFullKey(key);
        _cache.Set(fullKey, value);
    }

    public void Remove(string key)
    {
        var fullKey = GetFullKey(key);
        _cache.Remove(fullKey);
    }

    private string GetFullKey(string key)
    {
        return $"CS_{_cacheKeyBase}_{key}";
    }
}
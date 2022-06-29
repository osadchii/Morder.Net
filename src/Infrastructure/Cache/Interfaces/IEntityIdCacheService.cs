namespace Infrastructure.Cache.Interfaces;

// ReSharper disable once UnusedTypeParameter
public interface IEntityIdCacheService<T>
{
    bool TryGetValue(string key, out int value);
    void Set(string key, int value);
    void Remove(string key);
    bool TryGetValue(Guid key, out int value) => TryGetValue(key.ToString(), out value);

    void Set(Guid key, int value)
    {
        Set(key.ToString(), value);
    }

    void Remove(Guid key)
    {
        Remove(key.ToString());
    }
}
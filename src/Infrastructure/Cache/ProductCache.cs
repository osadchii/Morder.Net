using System.Net;
using Infrastructure.Cache.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Cache;

public class ProductCache : IProductCache
{
    private readonly MContext _context;
    private readonly IMemoryCache _cache;

    public ProductCache(MContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Dictionary<string, int>> GetProductIdsByArticul(List<string> articuls)
    {
        var result = new Dictionary<string, int>();
        var uncached = new List<string>();

        foreach (string articul in articuls)
        {
            if (_cache.TryGetValue(ArticulCacheKey(articul), out int id))
            {
                result.TryAdd(articul, id);
            }
            else
            {
                uncached.Add(articul);
            }
        }

        if (uncached.Count > 0)
        {
            Dictionary<string, int> fromContext = await _context.Products
                .AsNoTracking()
                .Where(p => p.Articul != null && uncached.Contains(p.Articul))
                .Select(p => new { p.Id, p.Articul })
                .ToDictionaryAsync(p => p.Articul!, p => p.Id);

            foreach (KeyValuePair<string, int> keyValue in fromContext)
            {
                result.TryAdd(keyValue.Key, keyValue.Value);
                _cache.Set(ArticulCacheKey(keyValue.Key), keyValue.Value);
            }
        }

        List<string> notFound = articuls.Except(result.Keys).ToList();

        if (notFound.Any())
        {
            throw new HttpRequestException($"Products with {string.Join(", ", notFound)} not found", null,
                HttpStatusCode.BadRequest);
        }

        return result;
    }

    private static string ArticulCacheKey(string articul)
    {
        return $"{CacheKeys.BaseProductArticulCacheKey}_{articul}";
    }
}
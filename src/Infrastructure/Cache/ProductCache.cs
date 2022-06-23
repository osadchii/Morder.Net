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

    public async Task<Dictionary<string, int>> GetProductIdsByArticul(List<string> articuls,
        bool ignoreNotFound = false)
    {
        var result = new Dictionary<string, int>();
        var uncached = new List<string>();
        var notFound = new List<string>();

        foreach (var articul in articuls)
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
            foreach (var articul in uncached)
            {
                var productId = await _context.Products
                    .AsNoTracking()
                    .Where(p =>
                        p.Articul != null && p.Articul == articul || p.Articul == $"0{articul}" ||
                        p.Articul == $"00{articul}")
                    .Select(p => p.Id)
                    .SingleOrDefaultAsync();

                if (productId == default)
                {
                    notFound.Add(articul);
                }
                else
                {
                    result.TryAdd(articul, productId);
                    _cache.Set(ArticulCacheKey(articul), productId);
                }

            }
        }

        if (ignoreNotFound)
        {
            return result;
        }

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
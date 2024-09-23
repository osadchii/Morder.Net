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
                var fullArticul = GetFullArticul(articul);
                var productId = await _context.Products
                    .AsNoTracking()
                    .Where(p =>
                        p.Articul != null && p.Articul == fullArticul)
                    .Select(p => p.Id)
                    .SingleOrDefaultAsync();

                if (productId == default)
                {
                    productId = await _context.Products
                        .AsNoTracking()
                        .Where(p =>
                            p.Articul != null && p.Articul == articul)
                        .Select(p => p.Id)
                        .SingleOrDefaultAsync();
                }

                if (productId == default)
                {
                    notFound.Add(articul);
                }
                else
                {
                    result.TryAdd(articul, productId);
                    _cache.Set(ArticulCacheKey(articul), productId, TimeSpan.FromHours(1));
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

    private static string GetFullArticul(string articul)
    {
        return articul.Length switch
        {
            1 => $"00000{articul}",
            2 => $"0000{articul}",
            3 => $"000{articul}",
            4 => $"00{articul}",
            5 => $"0{articul}",
            _ => articul
        };
    }

    private static string ArticulCacheKey(string articul)
    {
        return $"{CacheKeys.BaseProductArticulCacheKey}_{articul}";
    }
}
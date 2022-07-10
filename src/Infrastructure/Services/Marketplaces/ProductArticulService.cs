using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Services.Marketplaces;

public interface IProductArticulService
{
    Task<int> GetProductIdByArticul(string articul);
    Task<Dictionary<string, int>> GetProductIdsByArticuls(IEnumerable<string> articuls);
}

public class ProductArticulService : IProductArticulService
{
    private readonly MContext _context;
    private readonly IMemoryCache _cache;
    private const string CacheKeyTemplate = $"{nameof(ProductArticulService)}_Articul_{{0}}";
    
    public ProductArticulService(MContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<int> GetProductIdByArticul(string articul)
    {
        var cacheKey = GetCacheKey(articul);

        if (_cache.TryGetValue(cacheKey, out int id))
        {
            return id;
        }

        var productId = await _context
            .Products
            .AsNoTracking()
            .Where(p => p.Articul == articul)
            .Select(p => p.Id)
            .SingleOrDefaultAsync();

        if (productId != 0)
        {
            _cache.Set(cacheKey, productId);
        }

        return productId;
    }

    public async Task<Dictionary<string, int>> GetProductIdsByArticuls(IEnumerable<string> articuls)
    {
        var result = new Dictionary<string, int>();
        var notInCache = new List<string>();

        foreach (var articul in articuls)
        {
            var cacheKey = GetCacheKey(articul);

            if (_cache.TryGetValue(cacheKey, out int id))
            {
                result.Add(articul, id);
            }
            else
            {
                notInCache.Add(articul);
            }
        }

        Dictionary<string, int> productIds = await _context.Products
            .AsNoTracking()
            .Where(p => notInCache.Contains(p.Articul!))
            .Select(p => new { p.Articul, p.Id })
            .ToDictionaryAsync(p => p.Articul!, p => p.Id);

        foreach (KeyValuePair<string, int> kv in productIds)
        {
            var cacheKey = GetCacheKey(kv.Key);
            _cache.Set(cacheKey, kv.Value);
            
            result.Add(kv.Key, kv.Value);
        }

        return result;
    }

    private static string GetCacheKey(string articul)
    {
        return string.Format(CacheKeyTemplate, articul);
    }
}
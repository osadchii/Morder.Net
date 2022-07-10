using Infrastructure.Models.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.Marketplaces;

public interface IProductIdentifierService
{
    Task<bool> SetIdentifierAsync(int marketplaceId, int productId, ProductIdentifierType type, string value);

    Task<Dictionary<int, bool>> SetIdentifiersAsync(int marketplaceId, Dictionary<int, string> productIdentifiers,
        ProductIdentifierType type);
    Task<string?> GetIdentifierAsync(int marketplaceId, int productId, ProductIdentifierType type);

    Task<Dictionary<int, string?>> GetIdentifiersAsync(int marketplaceId, IEnumerable<int> productIds,
        ProductIdentifierType type);
}

public class ProductIdentifierService : IProductIdentifierService
{
    private readonly MContext _context;
    private readonly IMemoryCache _cache;
    private const string CacheKeyTemplate = $"{nameof(ProductIdentifierService)}_{{0}}_{{1}}_{{2}}";
    private readonly ILogger<ProductIdentifierService> _logger;

    public ProductIdentifierService(MContext context, IMemoryCache cache, ILogger<ProductIdentifierService> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    public async Task<bool> SetIdentifierAsync(int marketplaceId, int productId, ProductIdentifierType type, string value)
    {
        var cacheKey = GetCacheKey(marketplaceId, productId, type);

        if (_cache.TryGetValue(cacheKey, out string cacheValue) && cacheValue == value)
        {
            return false;
        }

        ProductIdentifier? currentValue = await _context.ProductIdentifiers
            .SingleOrDefaultAsync(pi =>
                pi.MarketplaceId == marketplaceId && pi.ProductId == productId && pi.Type == type);

        if (currentValue is null)
        {
            await _context.ProductIdentifiers.AddAsync(new ProductIdentifier
            {
                MarketplaceId = marketplaceId,
                ProductId = productId,
                Type = type,
                Value = value
            });
            _logger.LogInformation("Product identifier has been added for product {ProductId} id for marketplace {MarketplaceId} id with value {Value}",
                productId, marketplaceId, value);
        }
        else if (currentValue.Value != value)
        {
            currentValue.Value = value;
            _logger.LogInformation("Product identifier has been changed for product {ProductId} id for marketplace {MarketplaceId} id with value {Value}",
                productId, marketplaceId, value);
        }
        else
        {
            _cache.Set(cacheKey, value);
            return false;
        }

        await _context.SaveChangesAsync();
        _cache.Set(cacheKey, value);

        return true;
    }

    public async Task<Dictionary<int, bool>> SetIdentifiersAsync(int marketplaceId, Dictionary<int, string> productIdentifiers, ProductIdentifierType type)
    {
        var result = new Dictionary<int, bool>();

        foreach (KeyValuePair<int, string> kv in productIdentifiers)
        {
            result[kv.Key] = await SetIdentifierAsync(marketplaceId, kv.Key, type, kv.Value);
        }
        
        return result;
    }

    public async Task<string?> GetIdentifierAsync(int marketplaceId, int productId, ProductIdentifierType type)
    {
        var cacheKey = GetCacheKey(marketplaceId, productId, type);

        if (_cache.TryGetValue(cacheKey, out string cachedValue))
        {
            return cachedValue;
        }
        
        ProductIdentifier? currentValue = await _context.ProductIdentifiers
            .SingleOrDefaultAsync(pi =>
                pi.MarketplaceId == marketplaceId && pi.ProductId == productId && pi.Type == type);

        if (currentValue is null) 
            return null;
        
        _cache.Set(cacheKey, currentValue.Value);
        return currentValue.Value;
    }

    public async Task<Dictionary<int, string?>> GetIdentifiersAsync(int marketplaceId, IEnumerable<int> productIds, ProductIdentifierType type)
    {
        var result = new Dictionary<int, string?>();

        foreach (var productId in productIds)
        {
            result[productId] = await GetIdentifierAsync(marketplaceId, productId, type);
        }
        
        return result;
    }

    private static string GetCacheKey(int marketplaceId, int productId, ProductIdentifierType type)
    {
        return string.Format(CacheKeyTemplate, marketplaceId, productId, type);
    }
}
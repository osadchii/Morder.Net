using Infrastructure.Cache.Interfaces;
using Infrastructure.Models;
using Infrastructure.Models.Interfaces;
using Infrastructure.Models.Prices;
using Infrastructure.Models.Products;
using Infrastructure.Models.Warehouses;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Cache;

public static class MediatRDependencyInjection
{
    public static void AddCacheServices(this IServiceCollection services)
    {
        services.AddEntityCache<Product>();
        services.AddEntityCache<Warehouse>();
        services.AddEntityCache<PriceType>();
        services.AddEntityCache<Category>();
    }

    private static void AddEntityCache<T>(this IServiceCollection services) where T : BaseEntity, IHasId, IHasExternalId
    {
        services.AddSingleton<IEntityIdCacheService<T>, EntityIdCacheService<T>>();
        services.AddTransient<IIdExtractor<T>, IdExtractor<T>>();
    }
}
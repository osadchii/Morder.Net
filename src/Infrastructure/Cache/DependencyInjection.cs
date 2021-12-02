using Infrastructure.Cache.Interfaces;
using Infrastructure.Models.Products;
using Infrastructure.Models.Warehouses;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Cache;

public static class MediatRDependencyInjection
{
    public static void AddCacheServices(this IServiceCollection services)
    {
        services.AddProductCache();
        services.AddWarehouseCache();
    }

    private static void AddProductCache(this IServiceCollection services)
    {
        services.AddSingleton<IEntityIdCacheService<Product>, EntityIdCacheService<Product>>();
        services.AddTransient<IIdExtractor<Product>, IdExtractor<Product>>();
    }

    private static void AddWarehouseCache(this IServiceCollection services)
    {
        services.AddSingleton<IEntityIdCacheService<Warehouse>, EntityIdCacheService<Warehouse>>();
        services.AddTransient<IIdExtractor<Warehouse>, IdExtractor<Warehouse>>();
    }
}
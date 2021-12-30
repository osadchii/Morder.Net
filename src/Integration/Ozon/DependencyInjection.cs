using Integration.Ozon.Clients.LoadProducts;
using Integration.Ozon.Clients.Prices;
using Integration.Ozon.Clients.Stocks;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Ozon;

public static class DependencyInjection
{
    public static void AddOzon(this IServiceCollection services)
    {
        services.AddTransient<IOzonLoadProductIdsClient, OzonLoadProductIdsClient>();
        services.AddTransient<IOzonStockClient, OzonStockClient>();
        services.AddTransient<IOzonPriceClient, OzonPriceClient>();
    }
}
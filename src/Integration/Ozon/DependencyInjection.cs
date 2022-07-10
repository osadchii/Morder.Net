using Integration.Ozon.Clients.LoadProducts;
using Integration.Ozon.Clients.Orders;
using Integration.Ozon.Clients.Prices;
using Integration.Ozon.Clients.Stocks;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Ozon;

public static class DependencyInjection
{
    public static void AddOzon(this IServiceCollection services)
    {
        services.AddTransient<IOzonLoadProductIdentifiersClient, OzonLoadProductIdentifiersClient>();
        services.AddTransient<IOzonLoadOrderListClient, OzonLoadOrderListClient>();
        services.AddTransient<IOzonStockClient, OzonStockClient>();
        services.AddTransient<IOzonPriceClient, OzonPriceClient>();
        services.AddTransient<IOzonGetStickerClient, OzonGetStickerClient>();
        services.AddTransient<IOzonGetOrdersClient, OzonGetOrdersClient>();
        services.AddTransient<IOzonRejectOrderClient, OzonRejectOrderClient>();
        services.AddTransient<IOzonPackOrderClient, OzonPackOrderClient>();

        services.AddTransient<IOzonOrderAdapter, OzonOrderAdapter>();
    }
}
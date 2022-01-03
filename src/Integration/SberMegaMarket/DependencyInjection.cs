using Integration.SberMegaMarket.Clients.Interfaces;
using Integration.SberMegaMarket.Clients.Prices;
using Integration.SberMegaMarket.Clients.Stocks;
using Integration.SberMegaMarket.Orders;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.SberMegaMarket;

public static class DependencyInjection
{
    public static void AddSberMegaMarket(this IServiceCollection services)
    {
        services.AddTransient<ISberMegaMarketStockClient, SberMegaMarketStockClient>();
        services.AddTransient<ISberMegaMarketPriceClient, SberMegaMarketPriceClient>();
        services.AddTransient<ISberMegaMarketOrderAdapter, SberMegaMarketOrderAdapter>();
    }
}
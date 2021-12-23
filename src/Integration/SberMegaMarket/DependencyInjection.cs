using Integration.SberMegaMarket.Clients;
using Integration.SberMegaMarket.Clients.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.SberMegaMarket;

public static class DependencyInjection
{
    public static void AddSberMegaMarket(this IServiceCollection services)
    {
        services.AddTransient<ISberMegaMarketStockClient, SberMegaMarketStockClient>();
    }
}
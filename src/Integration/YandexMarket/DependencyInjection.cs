using Integration.YandexMarket.Clients;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.YandexMarket;

public static class DependencyInjection
{
    public static void AddYandexMarket(this IServiceCollection services)
    {
        services.AddTransient<IYandexMarketMpGetOrdersClient, YandexMarketMpGetOrdersClient>();
    }
}
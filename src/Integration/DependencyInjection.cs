using Integration.Common;
using Integration.Kuper;
using Integration.Meso;
using Integration.Ozon;
using Integration.SberMegaMarket;
using Integration.YandexMarket;
using Microsoft.Extensions.DependencyInjection;

namespace Integration;

public static class DependencyInjection
{
    public static void AddMarketplaces(this IServiceCollection services)
    {
        services.AddMarketplaceServices();
        services.AddSberMegaMarket();
        services.AddOzon();
        services.AddMeso();
        services.AddKuper();
        services.AddYandexMarket();
    }
}
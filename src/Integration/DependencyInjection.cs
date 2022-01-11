using Integration.Common;
using Integration.Meso;
using Integration.Ozon;
using Integration.SberMegaMarket;
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
    }
}
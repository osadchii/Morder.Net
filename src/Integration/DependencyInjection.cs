using Integration.Common;
using Integration.SberMegaMarket;
using Microsoft.Extensions.DependencyInjection;

namespace Integration;

public static class DependencyInjection
{
    public static void AddMarketplaces(this IServiceCollection services)
    {
        services.AddMarketplaceServices();
        services.AddSberMegaMarket();
    }
}
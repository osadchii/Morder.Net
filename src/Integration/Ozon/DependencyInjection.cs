using Integration.Ozon.Clients.LoadProducts;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Ozon;

public static class DependencyInjection
{
    public static void AddOzon(this IServiceCollection services)
    {
        services.AddTransient<IOzonLoadProductIdsClient, OzonLoadProductIdsClient>();
    }
}
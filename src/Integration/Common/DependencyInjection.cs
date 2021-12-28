using Integration.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Common;

public static class DependencyInjection
{
    public static void AddMarketplaceServices(this IServiceCollection services)
    {
        services.AddTransient<ISendStockService, SendStockService>();
        services.AddTransient<ISendPriceService, SendPriceService>();
        services.AddTransient<ILoadProductIdService, LoadProductIdService>();
    }
}
using Integration.Common.Services.Orders;
using Integration.Common.Services.Prices;
using Integration.Common.Services.Products;
using Integration.Common.Services.Stocks;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Common;

public static class DependencyInjection
{
    public static void AddMarketplaceServices(this IServiceCollection services)
    {
        services.AddTransient<ISendStockService, SendStockService>();
        services.AddTransient<ISendPriceService, SendPriceService>();
        services.AddTransient<ILoadProductIdService, LoadProductIdService>();
        services.AddTransient<ITaskHandleService, TaskHandleService>();
    }
}
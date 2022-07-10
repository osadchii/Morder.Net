using Integration.Common.Services.Feeds;
using Integration.Common.Services.Orders;
using Integration.Common.Services.Products;
using Integration.Common.Services.StocksAndPrices;
using Integration.Common.Services.StocksAndPrices.Prices;
using Integration.Common.Services.StocksAndPrices.Stocks;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Common;

public static class DependencyInjection
{
    public static void AddMarketplaceServices(this IServiceCollection services)
    {
        services.AddTransient<ISendStockService, SendStockService>();
        services.AddTransient<ISendPriceService, SendPriceService>();
        services.AddTransient<ILoadProductIdentifiersService, LoadProductIdentifiersService>();
        services.AddTransient<ITaskHandleService, TaskHandleService>();
        services.AddTransient<IUpdateOrdersService, UpdateOrdersService>();
        services.AddTransient<ILoadOrdersService, LoadOrdersService>();
        services.AddTransient<IFeedService, FeedService>();
        services.AddTransient<ITrackAllStocksAndPricesService, TrackAllStocksAndPricesService>();
    }
}
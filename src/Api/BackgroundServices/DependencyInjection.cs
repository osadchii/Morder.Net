using Api.BackgroundServices.Marketplaces;

namespace Api.BackgroundServices;

public static class DependencyInjection
{
    public static void AddBackgroundServices(this IServiceCollection services)
    {
        services.AddHostedService<MarketplaceFeedBackgroundService>();
        services.AddHostedService<SendStockBackgroundService>();
        services.AddHostedService<SendPriceBackgroundService>();
        services.AddHostedService<LoadProductIdsBackgroundService>();
        services.AddHostedService<MarketplaceOrderTaskExecutorService>();
        services.AddHostedService<UpdateOrdersBackgroundService>();
        services.AddHostedService<LoadOrdersBackgroundService>();
        services.AddHostedService<TrackAllStocksAndPricesBackgroundService>();
        services.AddHostedService<SendOrderConfirmationNotifications>();
    }
}
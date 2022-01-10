using Integration.Common.Services.Orders;

namespace Api.BackgroundServices.Marketplaces;

public class LoadOrdersBackgroundService : BackgroundService
{
    public LoadOrdersBackgroundService(ILogger<LoadOrdersBackgroundService> logger, IServiceProvider services,
        IConfiguration configuration)
        : base(logger, services, "Load marketplace orders")
    {
        TimerInterval = configuration.GetValue<int>("MarketplaceSettings:LoadOrdersInterval");
    }

    protected override async Task ServiceWork()
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        var loadProductIdService = scope.ServiceProvider.GetRequiredService<ILoadOrdersService>();

        await loadProductIdService.LoadOrders();
    }
}
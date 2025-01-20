using Integration.Common.Services.Orders;

namespace Api.BackgroundServices.Marketplaces;

public class UpdateOrdersBackgroundService : BackgroundService
{
    public UpdateOrdersBackgroundService(ILogger<UpdateOrdersBackgroundService> logger, IServiceProvider services,
        IConfiguration configuration)
        : base(logger, services)
    {
        TimerInterval = configuration.GetValue<int>("MarketplaceSettings:Orders:UpdateOrdersInterval");
    }

    protected override async Task ServiceWork()
    {
        await using var scope = Services.CreateAsyncScope();
        var service = scope.ServiceProvider.GetRequiredService<IUpdateOrdersService>();

        await service.UpdateOrders();
    }
}
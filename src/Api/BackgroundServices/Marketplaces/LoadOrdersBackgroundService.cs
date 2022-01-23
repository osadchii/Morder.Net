using Infrastructure.Extensions;
using Integration.Common.Services.Orders;

namespace Api.BackgroundServices.Marketplaces;

public class LoadOrdersBackgroundService : BackgroundService
{
    private readonly DateTime _startDate;
    public LoadOrdersBackgroundService(ILogger<LoadOrdersBackgroundService> logger, IServiceProvider services,
        IConfiguration configuration) : base(logger, services)
    {
        TimerInterval = configuration.GetValue<int>("MarketplaceSettings:Orders:LoadOrdersInterval");
        _startDate = configuration.GetValue<DateTime>("MarketplaceSettings:Orders:LoadOrdersStartDate");
    }

    protected override async Task ServiceWork()
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        var service = scope.ServiceProvider.GetRequiredService<ILoadOrdersService>();

        await service.LoadOrders(_startDate.ToUtcTime());
    }
}
using Infrastructure.Extensions;
using Integration.Common.Services.Orders;

namespace Api.BackgroundServices.Marketplaces;

public class LoadOrdersBackgroundService : BackgroundService
{
    private readonly DateTime _startDate;
    private readonly bool _loadOnlyNewOrders;
    private readonly int _loadNewOrdersDaysInterval;

    public LoadOrdersBackgroundService(ILogger<LoadOrdersBackgroundService> logger, IServiceProvider services,
        IConfiguration configuration) : base(logger, services)
    {
        TimerInterval = configuration.GetValue<int>("MarketplaceSettings:Orders:LoadOrdersInterval");
        _startDate = configuration.GetValue<DateTime>("MarketplaceSettings:Orders:LoadOrdersStartDate");

        _loadOnlyNewOrders = configuration.GetValue<bool>("MarketplaceSettings:Orders:LoadOnlyNewOrders");
        _loadNewOrdersDaysInterval =
            configuration.GetValue<int>("MarketplaceSettings:Orders:LoadNewOrdersDaysInterval");
    }

    protected override async Task ServiceWork()
    {
        await using var scope = Services.CreateAsyncScope();
        var service = scope.ServiceProvider.GetRequiredService<ILoadOrdersService>();

        if (_loadOnlyNewOrders)
        {
            await service.LoadOrdersInDays(_loadNewOrdersDaysInterval);
        }
        else
        {
            await service.LoadOrdersFromDate(_startDate.ToUtcTime());
        }
    }
}
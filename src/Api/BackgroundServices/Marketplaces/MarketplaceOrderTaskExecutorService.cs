using Integration.Common.Services.Orders;

namespace Api.BackgroundServices.Marketplaces;

public class MarketplaceOrderTaskExecutorService : BackgroundService
{
    private readonly int _maxTryCount;

    public MarketplaceOrderTaskExecutorService(ILogger<MarketplaceOrderTaskExecutorService> logger,
        IServiceProvider services, IConfiguration configuration)
        : base(logger, services, "Marketplace order task")
    {
        TimerInterval = configuration.GetValue<int>("MarketplaceSettings:Tasks:ExecutionInterval");
        _maxTryCount = configuration.GetValue<int>("MarketplaceSettings:Tasks:MaxTryCount");
    }

    protected override async Task ServiceWork()
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        var taskHandler = scope.ServiceProvider.GetRequiredService<ITaskHandleService>();

        await taskHandler.HandleTasks(_maxTryCount);
    }
}
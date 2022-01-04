using Integration.Common.Services.Orders;

namespace Api.BackgroundServices.Marketplaces;

public class MarketplaceOrderTaskExecutorService : IHostedService, IDisposable
{
    private readonly ILogger<MarketplaceOrderTaskExecutorService> _logger;
    private readonly int _taskExecutorInterval;
    private readonly int _maxTryCount;
    private readonly IServiceProvider _services;
    private Task? _task;

    private Timer _timer = null!;

    public MarketplaceOrderTaskExecutorService(IServiceProvider services,
        ILogger<MarketplaceOrderTaskExecutorService> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _services = services;

        _taskExecutorInterval = configuration.GetValue<int>("MarketplaceSettings:Tasks:ExecutionInterval");
        _maxTryCount = configuration.GetValue<int>("MarketplaceSettings:Tasks:MaxTryCount");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Marketplace order task service running.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromMinutes(_taskExecutorInterval));

        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        if (_task is null
            || _task.Status is TaskStatus.Canceled or TaskStatus.Faulted or TaskStatus.RanToCompletion)
        {
            _task = SendPrices();
        }
    }

    private async Task SendPrices()
    {
        await using AsyncServiceScope scope = _services.CreateAsyncScope();
        var taskHandler = scope.ServiceProvider.GetRequiredService<ITaskHandleService>();

        await taskHandler.HandleTasks(_maxTryCount);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Marketplace order task service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
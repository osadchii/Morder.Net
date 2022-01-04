using Integration.Common.Services.Orders;

namespace Api.BackgroundServices.Marketplaces;

public class LoadOrdersBackgroundService : IHostedService, IDisposable
{
    private readonly ILogger<LoadOrdersBackgroundService> _logger;
    private readonly int _loadOrdersInterval;
    private readonly IServiceProvider _services;
    private Task? _task;

    private Timer _timer = null!;

    public LoadOrdersBackgroundService(IServiceProvider services, ILogger<LoadOrdersBackgroundService> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _services = services;

        _loadOrdersInterval = configuration.GetValue<int>("MarketplaceSettings:LoadOrdersInterval");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Marketplace load orders service running.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromMinutes(_loadOrdersInterval));

        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        if (_task is null
            || _task.Status is TaskStatus.Canceled or TaskStatus.Faulted or TaskStatus.RanToCompletion)
        {
            _task = LoadProductIds();
        }
    }

    private async Task LoadProductIds()
    {
        await using AsyncServiceScope scope = _services.CreateAsyncScope();
        var loadProductIdService = scope.ServiceProvider.GetRequiredService<ILoadOrdersService>();

        await loadProductIdService.LoadOrders();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("MMarketplace load orders service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
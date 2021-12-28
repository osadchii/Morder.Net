using Integration.Common.Services;

namespace Api.BackgroundServices.Marketplaces;

public class LoadProductIdsBackgroundService : IHostedService, IDisposable
{
    private readonly ILogger<LoadProductIdsBackgroundService> _logger;
    private readonly int _loadProductIdsInterval;
    private readonly IServiceProvider _services;
    private Task? _task;

    private Timer _timer = null!;

    public LoadProductIdsBackgroundService(IServiceProvider services, ILogger<LoadProductIdsBackgroundService> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _services = services;

        _loadProductIdsInterval = configuration.GetValue<int>("LoadProductIdsInterval");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Marketplace load product ids service running.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromMinutes(_loadProductIdsInterval));

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
        var loadProductIdService = scope.ServiceProvider.GetRequiredService<ILoadProductIdService>();

        await loadProductIdService.LoadMarketplaceProductIds();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("MMarketplace load product ids service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
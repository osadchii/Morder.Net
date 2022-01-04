using Integration.Common.Services.Prices;

namespace Api.BackgroundServices.Marketplaces;

public class SendPriceBackgroundService : IHostedService, IDisposable
{
    private readonly ILogger<SendPriceBackgroundService> _logger;
    private readonly int _sendPriceInterval;
    private readonly IServiceProvider _services;
    private Task? _task;

    private Timer _timer = null!;

    public SendPriceBackgroundService(IServiceProvider services, ILogger<SendPriceBackgroundService> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _services = services;

        _sendPriceInterval = configuration.GetValue<int>("MarketplaceSettings:SendPriceInterval");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Marketplace send price service running.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromMinutes(_sendPriceInterval));

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
        var sendPriceService = scope.ServiceProvider.GetRequiredService<ISendPriceService>();

        await sendPriceService.SendMarketplacePrices();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Marketplace send price service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
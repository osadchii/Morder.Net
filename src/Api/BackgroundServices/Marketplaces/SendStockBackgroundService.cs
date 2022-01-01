using System;
using System.Threading;
using System.Threading.Tasks;
using Integration.Common.Services.Stocks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Api.BackgroundServices.Marketplaces;

public class SendStockBackgroundService : IHostedService, IDisposable
{
    private readonly ILogger<SendStockBackgroundService> _logger;
    private readonly int _sendStockInterval;
    private readonly IServiceProvider _services;
    private Task? _task;

    private Timer _timer = null!;

    public SendStockBackgroundService(IServiceProvider services, ILogger<SendStockBackgroundService> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _services = services;

        _sendStockInterval = configuration.GetValue<int>("SendStockInterval");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Marketplace send stock service running.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromMinutes(_sendStockInterval));

        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        if (_task is null
            || _task.Status is TaskStatus.Canceled or TaskStatus.Faulted or TaskStatus.RanToCompletion)
        {
            _task = GenerateFeeds();
        }
    }

    private async Task GenerateFeeds()
    {
        await using AsyncServiceScope scope = _services.CreateAsyncScope();
        var sendStockService = scope.ServiceProvider.GetRequiredService<ISendStockService>();

        await sendStockService.SendMarketplaceStocks();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Marketplace send stock service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
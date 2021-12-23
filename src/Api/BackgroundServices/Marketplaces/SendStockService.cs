using Infrastructure;
using Infrastructure.MediatR.Stocks.Queries;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Warehouses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.BackgroundServices.Marketplaces;

public class SendStockService : IHostedService, IDisposable
{
    private readonly ILogger<SendStockService> _logger;
    private readonly int _sendStockInterval;
    private IServiceProvider Services { get; }
    private Task? _task;

    private Timer _timer = null!;

    public SendStockService(IServiceProvider services, ILogger<SendStockService> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        Services = services;

        _sendStockInterval = configuration.GetValue<int>("SendStockInterval");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Marketplace send stock service running.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(_sendStockInterval));

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
        await using AsyncServiceScope scope = Services.CreateAsyncScope();

        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var context = scope.ServiceProvider.GetRequiredService<MContext>();
        try
        {
            List<Marketplace> marketplaces = await context.Marketplaces
                .AsNoTracking()
                .Where(m => m.IsActive && m.StockChangesTracking)
                .ToListAsync();

            foreach (Marketplace marketplace in marketplaces)
            {
                IEnumerable<MarketplaceStockDto> stocks = await mediator.Send(new GetMarketplaceStocksRequest
                {
                    MarketplaceId = marketplace.Id,
                    Limit = marketplace.StockSendLimit
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while sending stocks");
        }
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
using Infrastructure.MediatR.Marketplaces.Common.Queries;
using Infrastructure.MediatR.Marketplaces.SberMegaMarket.Queries;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Marketplace.SberMegaMarket.Extensions;
using Marketplace.SberMegaMarket.Feeds;
using MediatR;

namespace Api.BackgroundServices;

public class SberMegaMarketFeedService : IHostedService, IDisposable
{
    private readonly ILogger<SberMegaMarketFeedService> _logger;
    private readonly int _feedGenerationInterval;
    private IServiceProvider Services { get; }
    private Task? _task;

    private Timer _timer = null!;

    public SberMegaMarketFeedService(IServiceProvider services, ILogger<SberMegaMarketFeedService> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        Services = services;

        _feedGenerationInterval = configuration.GetValue<int>("FeedGenerationInterval");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("SberMegaMarket feed service running.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromMinutes(_feedGenerationInterval));

        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        if (_task is null
            || _task.Status == TaskStatus.Canceled
            || _task.Status == TaskStatus.Faulted
            || _task.Status == TaskStatus.RanToCompletion)
        {
            _task = GenerateFeeds();
            _logger.LogInformation("Task started");
        }
        else
        {
            _logger.LogInformation("Skip starting");
        }
    }

    private async Task GenerateFeeds()
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();

        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        try
        {
            List<SberMegaMarketDto> marketplaces =
                await mediator.Send(new GetAllActiveSberMegaMarketSettingsRequest());

            foreach (SberMegaMarketDto marketplace in marketplaces)
            {
                if (!marketplace.Settings.FeedEnabled)
                {
                    continue;
                }

                MarketplaceProductData data = await mediator.Send(new GetMarketplaceProductDataRequest()
                {
                    MarketplaceId = marketplace.Id
                });

                var feedBuilder = new FeedBuilder(data, marketplace, _logger);
                Feed feed = feedBuilder.Build();

                string feedPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "feeds");
                string path = Path.Combine(feedPath, $"{marketplace.Settings.FeedName}.xml");
                if (!Directory.Exists(feedPath))
                {
                    Directory.CreateDirectory(feedPath);
                }

                feed.Save(path);
                _logger.LogInformation($"Saved feed {path}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while generating SberMegaMarket feed");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("SberMegaMarket feed service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
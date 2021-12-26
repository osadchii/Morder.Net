using System.Diagnostics;
using Infrastructure.MediatR.Companies.Queries;
using Infrastructure.MediatR.Marketplaces.Common.Queries;
using Infrastructure.MediatR.Marketplaces.SberMegaMarket.Queries;
using Infrastructure.Models.Companies;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Integration.SberMegaMarket.Extensions;
using Integration.SberMegaMarket.Feeds;
using MediatR;

namespace Api.BackgroundServices.Marketplaces.SberMegaMarketServices;

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
            || _task.Status is TaskStatus.Canceled or TaskStatus.Faulted or TaskStatus.RanToCompletion)
        {
            _task = GenerateFeeds();
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

            CompanyDto companyInformation = await mediator.Send(new GetCompanyInformationRequest());

            foreach (SberMegaMarketDto marketplace in marketplaces)
            {
                if (!marketplace.Settings.FeedEnabled)
                {
                    continue;
                }

                var sw = new Stopwatch();
                sw.Start();

                MarketplaceProductData data = await mediator.Send(new GetMarketplaceProductDataRequest()
                {
                    MarketplaceId = marketplace.Id
                });

                FeedBuilder feedBuilder = new FeedBuilder(data, marketplace, _logger)
                    .AddCompanyInformation(companyInformation);

                Feed feed = feedBuilder.Build();

                string feedPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "feeds");
                string path = Path.Combine(feedPath, $"{marketplace.Settings.FeedName}.xml");
                if (!Directory.Exists(feedPath))
                {
                    Directory.CreateDirectory(feedPath);
                }

                feed.Save(path);

                sw.Stop();
                _logger.LogInformation($"Saved feed {path}. Elapsed: {sw.ElapsedMilliseconds} ms");
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
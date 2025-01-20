using Integration.Common.Services.Feeds;

namespace Api.BackgroundServices.Marketplaces;

public class MarketplaceFeedBackgroundService : BackgroundService
{
    public MarketplaceFeedBackgroundService(ILogger<MarketplaceFeedBackgroundService> logger, IServiceProvider services,
        IConfiguration configuration)
        : base(logger, services)
    {
        TimerInterval = configuration.GetValue<int>("MarketplaceSettings:FeedGenerationInterval");
    }

    protected override async Task ServiceWork()
    {
        await using var scope = Services.CreateAsyncScope();
        var feedService = scope.ServiceProvider.GetRequiredService<IFeedService>();

        await feedService.GenerateFeeds();
    }
}
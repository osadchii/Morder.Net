using Integration.Common.Services.Products;

namespace Api.BackgroundServices.Marketplaces;

public class LoadProductIdsBackgroundService : BackgroundService
{
    public LoadProductIdsBackgroundService(ILogger<LoadProductIdsBackgroundService> logger, IServiceProvider services,
        IConfiguration configuration)
        : base(logger, services)
    {
        TimerInterval = configuration.GetValue<int>("MarketplaceSettings:LoadProductIdsInterval");
    }

    protected override async Task ServiceWork()
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        var loadProductIdService = scope.ServiceProvider.GetRequiredService<ILoadProductIdService>();

        await loadProductIdService.LoadMarketplaceProductIds();
    }
}
using Integration.Common.Services.StocksAndPrices.Prices;

namespace Api.BackgroundServices.Marketplaces;

public class SendPriceBackgroundService : BackgroundService
{
    public SendPriceBackgroundService(ILogger<SendPriceBackgroundService> logger, IServiceProvider services,
        IConfiguration configuration)
        : base(logger, services)
    {
        TimerInterval = configuration.GetValue<int>("MarketplaceSettings:SendPriceInterval");
    }

    protected override async Task ServiceWork()
    {
        await using var scope = Services.CreateAsyncScope();
        var sendPriceService = scope.ServiceProvider.GetRequiredService<ISendPriceService>();

        await sendPriceService.SendMarketplacePrices();
    }
}
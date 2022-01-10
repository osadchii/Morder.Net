using Integration.Common.Services.Prices;

namespace Api.BackgroundServices.Marketplaces;

public class SendPriceBackgroundService : BackgroundService
{
    public SendPriceBackgroundService(ILogger<SendPriceBackgroundService> logger, IServiceProvider services,
        IConfiguration configuration)
        : base(logger, services, "Send prices")
    {
        TimerInterval = configuration.GetValue<int>("MarketplaceSettings:SendPriceInterval");
    }

    protected override async Task ServiceWork()
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        var sendPriceService = scope.ServiceProvider.GetRequiredService<ISendPriceService>();

        await sendPriceService.SendMarketplacePrices();
    }
}
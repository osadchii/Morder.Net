using Integration.Common.Services.StocksAndPrices.Stocks;

namespace Api.BackgroundServices.Marketplaces;

public class SendStockBackgroundService : BackgroundService
{
    public SendStockBackgroundService(ILogger<SendStockBackgroundService> logger, IServiceProvider services,
        IConfiguration configuration)
        : base(logger, services)
    {
        TimerInterval = configuration.GetValue<int>("MarketplaceSettings:SendStockInterval");
    }

    protected override async Task ServiceWork()
    {
        await using var scope = Services.CreateAsyncScope();
        var sendStockService = scope.ServiceProvider.GetRequiredService<ISendStockService>();

        await sendStockService.SendMarketplaceStocks();
    }
}
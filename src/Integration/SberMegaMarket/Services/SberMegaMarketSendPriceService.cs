using AutoMapper;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Infrastructure.Models.Prices;
using Integration.Common.Services.StocksAndPrices.Prices;
using Integration.SberMegaMarket.Clients;
using Integration.SberMegaMarket.Clients.Prices.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.SberMegaMarket.Services;

public class SberMegaMarketSendPriceService : MarketplaceSendPriceService
{
    public SberMegaMarketSendPriceService(IMapper mapper, IServiceProvider serviceProvider) : base(mapper,
        serviceProvider)
    {
    }

    public override Task SendPricesAsync(Marketplace marketplace, MarketplacePriceDto[] prices)
    {
        var client = ServiceProvider.GetRequiredService<ISberMegaMarketClient<SendPriceData>>();
        var sber = Mapper.Map<SberMegaMarketDto>(marketplace);
        var request = new SberMegaMarketMessage<SendPriceData>(sber.Settings.Token);

        foreach (var stock in prices)
        {
            request.Data.Prices.Add(new SberMegaMarketPrice(stock.Articul, stock.Value));
        }

        return client.SendRequest(ApiUrls.SendPrices, sber, request);
    }
}
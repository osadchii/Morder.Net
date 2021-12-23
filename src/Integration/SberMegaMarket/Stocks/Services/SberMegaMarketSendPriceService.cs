using AutoMapper;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Infrastructure.Models.Prices;
using Integration.Common.Services;
using Integration.SberMegaMarket.Clients.Interfaces;
using Integration.SberMegaMarket.Stocks.Messages;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.SberMegaMarket.Stocks.Services;

public class SberMegaMarketSendPriceService : MarketplaceSendPriceService
{
    public SberMegaMarketSendPriceService(IMediator mediator, IMapper mapper, IServiceProvider serviceProvider) : base(
        mediator, mapper, serviceProvider)
    {
    }

    public override Task SendPricesAsync(Marketplace marketplace, IEnumerable<MarketplacePriceDto> stocks)
    {
        var client = ServiceProvider.GetRequiredService<ISberMegaMarketPriceClient>();
        var sber = Mapper.Map<SberMegaMarketDto>(marketplace);
        var request = new SberMegaMarketMessage<SberMegaMarketSendPriceData>(sber.Settings.Token);

        foreach (MarketplacePriceDto stock in stocks)
        {
            request.Data.Prices.Add(new SberMegaMarketPrice(stock.ProductExternalId, stock.Value));
        }

        return client.SendPrices(sber, request);
    }
}
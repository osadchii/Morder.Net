using AutoMapper;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Infrastructure.Models.Warehouses;
using Integration.Common.Services.Stocks;
using Integration.SberMegaMarket.Clients;
using Integration.SberMegaMarket.Clients.Interfaces;
using Integration.SberMegaMarket.Clients.Stocks.Messages;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.SberMegaMarket.Services;

public class SberMegaMarketSendStockService : MarketplaceSendStockService
{
    public SberMegaMarketSendStockService(IMediator mediator, IMapper mapper, IServiceProvider serviceProvider) : base(
        mediator, mapper, serviceProvider)
    {
    }

    public override Task SendStocksAsync(Marketplace marketplace, IEnumerable<MarketplaceStockDto> stocks)
    {
        var client = ServiceProvider.GetRequiredService<ISberMegaMarketClient<SendStockData>>();
        var sber = Mapper.Map<SberMegaMarketDto>(marketplace);
        var request = new SberMegaMarketMessage<SendStockData>(sber.Settings.Token);

        foreach (MarketplaceStockDto stock in stocks)
        {
            request.Data.Stocks.Add(new SberMegaMarketStock(stock.ProductExternalId, stock.Value));
        }

        return client.SendRequest(ApiUrls.SendStocks, sber, request);
    }
}
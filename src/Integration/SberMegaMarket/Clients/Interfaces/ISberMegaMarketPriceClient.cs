using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Integration.SberMegaMarket.Stocks.Messages;

namespace Integration.SberMegaMarket.Clients.Interfaces;

public interface ISberMegaMarketPriceClient
{
    Task SendPrices(SberMegaMarketDto sber, SberMegaMarketMessage<SberMegaMarketSendPriceData> stocks);
}
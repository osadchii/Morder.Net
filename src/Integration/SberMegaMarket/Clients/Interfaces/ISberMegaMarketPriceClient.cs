using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Integration.SberMegaMarket.Clients.Prices.Messages;

namespace Integration.SberMegaMarket.Clients.Interfaces;

public interface ISberMegaMarketPriceClient
{
    Task SendPrices(SberMegaMarketDto sber, SberMegaMarketMessage<SberMegaMarketSendPriceData> stocks);
}
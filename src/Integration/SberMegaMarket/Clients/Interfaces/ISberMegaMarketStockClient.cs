using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Integration.SberMegaMarket.Clients.Stocks.Messages;

namespace Integration.SberMegaMarket.Clients.Interfaces;

public interface ISberMegaMarketStockClient
{
    Task SendStocks(SberMegaMarketDto sber, SberMegaMarketMessage<SberMegaMarketSendStockData> stocks);
}
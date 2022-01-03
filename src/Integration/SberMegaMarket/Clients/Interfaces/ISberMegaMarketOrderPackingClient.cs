using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Integration.SberMegaMarket.Clients.Orders.Messages;

namespace Integration.SberMegaMarket.Clients.Interfaces;

public interface ISberMegaMarketOrderPackingClient
{
    Task SendRequest(SberMegaMarketDto sber, SberMegaMarketMessage<SberMegaMarketOrderPackingData> request);
}
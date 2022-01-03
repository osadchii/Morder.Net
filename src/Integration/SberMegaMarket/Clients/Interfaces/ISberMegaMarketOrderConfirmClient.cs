using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Integration.SberMegaMarket.Clients.Orders.Messages;

namespace Integration.SberMegaMarket.Clients.Interfaces;

public interface ISberMegaMarketOrderConfirmClient
{
    Task SendRequest(SberMegaMarketDto sber, SberMegaMarketMessage<SberMegaMarketOrderConfirmData> request);
}
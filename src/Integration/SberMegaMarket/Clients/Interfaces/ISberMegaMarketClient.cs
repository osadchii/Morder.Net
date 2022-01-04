using Infrastructure.Models.Marketplaces.SberMegaMarket;

namespace Integration.SberMegaMarket.Clients.Interfaces;

public interface ISberMegaMarketClient<T> where T : SberMegaMarketMessageData, new()
{
    Task<string> SendRequest(string url, SberMegaMarketDto sber, SberMegaMarketMessage<T> request);
}
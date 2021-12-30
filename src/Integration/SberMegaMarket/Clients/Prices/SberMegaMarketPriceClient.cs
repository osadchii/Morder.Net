using System.Net;
using System.Net.Http.Json;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Integration.SberMegaMarket.Clients.Interfaces;
using Integration.SberMegaMarket.Clients.Prices.Messages;

namespace Integration.SberMegaMarket.Clients.Prices;

public class SberMegaMarketPriceClient : ISberMegaMarketPriceClient
{
    private readonly HttpClient _client;

    public SberMegaMarketPriceClient()
    {
        _client = new HttpClient();
    }

    public async Task SendPrices(SberMegaMarketDto sber, SberMegaMarketMessage<SberMegaMarketSendPriceData> prices)
    {
        string url =
            $"https://{sber.Settings.Server}:{sber.Settings.Port}/api/merchantIntegration/v1/offerService/manualPrice/save";
        HttpResponseMessage responseMessage = await _client.PostAsync(url, JsonContent.Create(prices));

        if (responseMessage.StatusCode != HttpStatusCode.OK)
        {
            string body = await responseMessage.Content.ReadAsStringAsync();
            string message = $"Send SberMegaMarket price failure." +
                             $"{Environment.NewLine}Status code: ${responseMessage.StatusCode}" +
                             $"{Environment.NewLine}Message: {body}";

            throw new Exception(message);
        }
    }
}
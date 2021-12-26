using System.Net;
using System.Net.Http.Json;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Integration.SberMegaMarket.Clients.Interfaces;
using Integration.SberMegaMarket.Stocks.Messages;

namespace Integration.SberMegaMarket.Clients;

public class SberMegaMarketStockClient : ISberMegaMarketStockClient
{
    private readonly HttpClient _client;

    public SberMegaMarketStockClient()
    {
        _client = new HttpClient();
    }

    public async Task SendStocks(SberMegaMarketDto sber, SberMegaMarketMessage<SberMegaMarketSendStockData> stocks)
    {
        string url =
            $"https://{sber.Settings.Server}:{sber.Settings.Port}/api/merchantIntegration/v1/offerService/stock/update";
        HttpResponseMessage responseMessage = await _client.PostAsync(url, JsonContent.Create(stocks));

        if (responseMessage.StatusCode != HttpStatusCode.OK)
        {
            string body = await responseMessage.Content.ReadAsStringAsync();
            string message = $"Send SberMegaMarket stock failure." +
                             $"{Environment.NewLine}Status code: ${responseMessage.StatusCode}" +
                             $"{Environment.NewLine}Message: {body}";

            throw new Exception(message);
        }
    }
}
using System.Net;
using System.Net.Http.Json;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Integration.SberMegaMarket.Clients.Interfaces;

namespace Integration.SberMegaMarket.Clients;

public class SberMegaMarketClient<T> : ISberMegaMarketClient<T> where T : SberMegaMarketMessageData, new()
{
    private readonly HttpClient _client;

    public SberMegaMarketClient()
    {
        _client = new HttpClient();
    }

    public async Task SendRequest(string url, SberMegaMarketDto sber, SberMegaMarketMessage<T> request)
    {
        string fullUrl =
            $"https://{sber.Settings.Server}:{sber.Settings.Port}{url}";
        HttpResponseMessage responseMessage = await _client.PostAsync(fullUrl, JsonContent.Create(request));

        if (responseMessage.StatusCode != HttpStatusCode.OK)
        {
            string body = await responseMessage.Content.ReadAsStringAsync();
            string message = $"Send SberMegaMarket request failure." +
                             $"{Environment.NewLine}Url: ${fullUrl}" +
                             $"{Environment.NewLine}Status code: ${responseMessage.StatusCode}" +
                             $"{Environment.NewLine}Message: {body}";

            throw new Exception(message);
        }
    }
}
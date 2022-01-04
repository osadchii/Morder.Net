using System.Net;
using System.Text;
using Infrastructure.Extensions;
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

    public async Task<string> SendRequest(string url, SberMegaMarketDto sber, SberMegaMarketMessage<T> request)
    {
        string fullUrl =
            $"https://{sber.Settings.Server}:{sber.Settings.Port}{url}";

        string json = request.ToJson();
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage responseMessage = await _client.PostAsync(fullUrl, content);
        string body = await responseMessage.Content.ReadAsStringAsync();

        if (responseMessage.StatusCode == HttpStatusCode.OK)
        {
            return body;
        }

        string message = $"Send SberMegaMarket request failure." +
                         $"{Environment.NewLine}Url: {fullUrl}" +
                         $"{Environment.NewLine}Status code: {responseMessage.StatusCode}" +
                         $"{Environment.NewLine}Request: {json}" +
                         $"{Environment.NewLine}Response: {body}";

        throw new Exception(message);
    }
}
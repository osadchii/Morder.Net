using System.Net;
using System.Text;
using Infrastructure.Extensions;
using Infrastructure.Models.Marketplaces.SberMegaMarket;

namespace Integration.SberMegaMarket.Clients;

public interface ISberMegaMarketClient<T> where T : SberMegaMarketMessageData, new()
{
    Task<string> SendRequest(string url, SberMegaMarketDto sber, SberMegaMarketMessage<T> request);
}

public class SberMegaMarketClient<T> : ISberMegaMarketClient<T> where T : SberMegaMarketMessageData, new()
{
    private readonly HttpClient _client;

    public SberMegaMarketClient()
    {
        _client = new HttpClient();
    }

    public async Task<string> SendRequest(string url, SberMegaMarketDto sber, SberMegaMarketMessage<T> request)
    {
        var fullUrl =
            $"https://{sber.Settings.Server}:{sber.Settings.Port}{url}";

        var json = request.ToJson();
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var responseMessage = await _client.PostAsync(fullUrl, content);
        var body = await responseMessage.Content.ReadAsStringAsync();

        if (responseMessage.StatusCode == HttpStatusCode.OK)
        {
            return body;
        }

        var message = $"Send SberMegaMarket request failure." +
                      $"{Environment.NewLine}Url: {fullUrl}" +
                      $"{Environment.NewLine}Status code: {responseMessage.StatusCode}" +
                      $"{Environment.NewLine}Request: {json}" +
                      $"{Environment.NewLine}Response: {body}";

        throw new Exception(message);
    }
}
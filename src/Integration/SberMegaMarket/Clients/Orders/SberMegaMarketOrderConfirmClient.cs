using System.Net;
using System.Net.Http.Json;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Integration.SberMegaMarket.Clients.Interfaces;
using Integration.SberMegaMarket.Clients.Orders.Messages;

namespace Integration.SberMegaMarket.Clients.Orders;

public class SberMegaMarketOrderConfirmClient : ISberMegaMarketOrderConfirmClient
{
    private readonly HttpClient _client;

    public SberMegaMarketOrderConfirmClient()
    {
        _client = new HttpClient();
    }

    public async Task SendRequest(SberMegaMarketDto sber, SberMegaMarketMessage<SberMegaMarketOrderConfirmData> request)
    {
        string url =
            $"https://{sber.Settings.Server}:{sber.Settings.Port}/api/order/confirm";
        HttpResponseMessage responseMessage = await _client.PostAsync(url, JsonContent.Create(request));

        if (responseMessage.StatusCode != HttpStatusCode.OK)
        {
            string body = await responseMessage.Content.ReadAsStringAsync();
            string message = $"Send SberMegaMarket confirm request failure." +
                             $"{Environment.NewLine}Status code: ${responseMessage.StatusCode}" +
                             $"{Environment.NewLine}Message: {body}";

            throw new Exception(message);
        }
    }
}
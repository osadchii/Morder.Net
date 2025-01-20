using System.Net;
using Infrastructure.Extensions;
using Infrastructure.Models.Marketplaces.YandexMarket;
using Integration.YandexMarket.Clients.Messages;

namespace Integration.YandexMarket.Clients;

public interface IYandexMarketMpGetOrdersClient
{
    Task<MpOrder[]> GetOrders(YandexMarketDto yandexMarketDto);
}

public class YandexMarketMpGetOrdersClient : IYandexMarketMpGetOrdersClient
{
    private readonly HttpClient _client;

    public YandexMarketMpGetOrdersClient()
    {
        _client = new HttpClient();
    }

    public async Task<MpOrder[]> GetOrders(YandexMarketDto yandexMarketDto)
    {
        const string fullUrl = "http://mporders.osadchiidev.ru/api/v1/order/GetOrdersByMarketplace/?marketplaceId=2";

        var responseMessage = await _client.GetAsync(fullUrl);
        var body = await responseMessage.Content.ReadAsStringAsync();

        if (responseMessage.StatusCode == HttpStatusCode.OK)
        {
            var result = body.FromJson<MpOrder[]>();

            if (result is null)
            {
                throw new Exception($"Cant deserialize response body: {body}");
            }

            return result;
        }

        var message = $"Get Mp Orders request failure." +
                      $"{Environment.NewLine}Url: {fullUrl}" +
                      $"{Environment.NewLine}Status code: {responseMessage.StatusCode}" +
                      $"{Environment.NewLine}Response: {body}";

        throw new Exception(message);
    }
}
using System.Net;
using System.Net.Http.Json;
using Infrastructure.Extensions;
using Infrastructure.Models.Marketplaces.Ozon;
using Integration.Ozon.Clients.LoadProducts.Messages;

namespace Integration.Ozon.Clients.LoadProducts;

public interface IOzonLoadProductIdsClient
{
    public Task<Dictionary<string, string>> LoadProductIdsAsync(OzonDto ozon);
}

public class OzonLoadProductIdsClient : IOzonLoadProductIdsClient
{
    private readonly HttpClient _client;

    public OzonLoadProductIdsClient()
    {
        _client = new HttpClient();
    }

    public async Task<Dictionary<string, string>> LoadProductIdsAsync(OzonDto ozon)
    {
        var result = new Dictionary<string, string>();
        var currentPage = 1;
        var loaded = false;
        var pageLimit = 500;

        while (!loaded)
        {
            int total = await LoadProductPageAsync(ozon, result, currentPage++);
            if (result.Count >= total || currentPage > pageLimit)
            {
                loaded = true;
            }
        }

        return result;
    }

    private async Task<int> LoadProductPageAsync(OzonDto ozon, IDictionary<string, string> result, int page)
    {
        string url =
            $"https://{ozon.Settings.Server}:{ozon.Settings.Port}/v1/product/list";

        var request = new OzonProductIdsRequest
        {
            Page = page,
            PageSize = ozon.Settings.LoadProductIdsPageSize
        };

        var httpMessage = new HttpRequestMessage(HttpMethod.Post, url);

        httpMessage.Content = JsonContent.Create(request);
        httpMessage.Headers.Add("Client-Id", ozon.Settings.ClientId);
        httpMessage.Headers.Add("Api-Key", ozon.Settings.ApiKey);

        HttpResponseMessage httpResponse = await _client.SendAsync(httpMessage);
        string body = await httpResponse.Content.ReadAsStringAsync();

        if (httpResponse.StatusCode != HttpStatusCode.OK)
        {
            string message = $"Ozon update product ids error." +
                             $"{Environment.NewLine}Status code: ${httpResponse.StatusCode}" +
                             $"{Environment.NewLine}Message: {body}";

            throw new Exception(message);
        }

        var response = body.FromJson<OzonProductIdsResponse>();

        if (response is null)
        {
            string message = $"Ozon update product ids error." +
                             $"{Environment.NewLine}Can't deserialize body: {body}";

            throw new Exception(message);
        }

        foreach (OzonProductIdsResponseItem item in response.Result.Items)
        {
            result.TryAdd(item.OfferId, item.ProductId.ToString());
        }

        return response.Result.Total;
    }
}
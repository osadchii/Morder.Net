using Infrastructure.Extensions;
using Infrastructure.Models.Marketplaces.Ozon;
using Integration.Ozon.Clients.LoadProducts.Messages;
using Integration.Ozon.Clients.LoadProducts.Models;

namespace Integration.Ozon.Clients.LoadProducts;

public interface IOzonLoadProductIdentifiersClient
{
    public Task<Dictionary<string, OzonProductIdentifier>> LoadOzonProductIdentifiersAsync(OzonDto ozon);
}

public class OzonLoadProductIdentifiersClient : BaseOzonClient, IOzonLoadProductIdentifiersClient
{
    public async Task<Dictionary<string, OzonProductIdentifier>> LoadOzonProductIdentifiersAsync(OzonDto ozon)
    {
        var baseIdentifiers = await LoadOzonBaseIdentifiersAsync(ozon);
        var result = new Dictionary<string, OzonProductIdentifier>();
        const int limit = 1000;

        for (var i = 0; i < baseIdentifiers.Count; i += limit)
        {
            var portion = baseIdentifiers.Skip(i).Take(limit);
            var portionResult = await LoadOzonIdentifiersAsync(ozon, portion);

            foreach (var item in portionResult)
            {
                result.Add(item.Key, item.Value);
            }
        }

        return result;
    }

    private static async Task<Dictionary<string, OzonProductIdentifier>> LoadOzonIdentifiersAsync(OzonDto ozon, 
        IEnumerable<KeyValuePair<string, long>> portion)
    {
        var request = new OzonProductInfoRequest
        {
            ProductIds = portion.Select(kv => kv.Value)
        };

        var httpResponse = await PostAsync(ozon, "v2/product/info/list", request);
        var body = await httpResponse.Content.ReadAsStringAsync();

        var response = body.FromJson<OzonProductInfoResponse>();

        if (response is null)
        {
            var message = $"Ozon update product identifiers error." +
                          $"{Environment.NewLine}Can't deserialize body: {body}";

            throw new Exception(message);
        }

        return response.Items.ToDictionary(i => i.OfferId, i => new OzonProductIdentifier
        {
            OzonId = i.Id.ToString(),
            FboSku = i.FboSku.ToString(),
            FbsSku = i.FbsSku.ToString()
        });
    }

    private static async Task<Dictionary<string, long>> LoadOzonBaseIdentifiersAsync(OzonDto ozon)
    {
        var result = new Dictionary<string, long>();
        var lastId = string.Empty;
        var currentPage = 0;
        var loaded = false;
        const int pageLimit = 500;

        while (!loaded)
        {
            var response = await LoadProductPageAsync(ozon, result, lastId);
            lastId = response.Item2;
            
            if (result.Count >= response.Item1 || currentPage++ > pageLimit)
            {
                loaded = true;
            }
        }

        return result;
    }

    private static async Task<(int, string)> LoadProductPageAsync(OzonDto ozon, IDictionary<string, long> result, string lastId)
    {
        var request = new OzonProductIdsRequest
        {
            LastId = lastId,
            Limit = ozon.Settings.LoadProductIdsPageSize
        };

        var httpResponse = await PostAsync(ozon, "v2/product/list", request);
        var body = await httpResponse.Content.ReadAsStringAsync();

        var response = body.FromJson<OzonProductIdsResponse>();

        if (response is null)
        {
            var message = $"Ozon update product ids error." +
                          $"{Environment.NewLine}Can't deserialize body: {body}";

            throw new Exception(message);
        }

        foreach (var item in response.Result.Items)
        {
            result.TryAdd(item.OfferId, item.ProductId);
        }

        return (response.Result.Total, response.Result.LastId);
    }
}
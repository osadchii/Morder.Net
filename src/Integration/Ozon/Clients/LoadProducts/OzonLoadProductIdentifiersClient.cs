using Infrastructure.Extensions;
using Infrastructure.Models.Marketplaces.Ozon;
using Integration.Ozon.Clients.LoadProducts.Messages;
using Integration.Ozon.Clients.LoadProducts.Models;
using Microsoft.Extensions.Logging;

namespace Integration.Ozon.Clients.LoadProducts;

public interface IOzonLoadProductIdentifiersClient
{
    public Task<Dictionary<string, OzonProductIdentifier>> LoadOzonProductIdentifiersAsync(OzonDto ozon);
}

public class OzonLoadProductIdentifiersClient : BaseOzonClient, IOzonLoadProductIdentifiersClient
{
    private readonly ILogger<OzonLoadProductIdentifiersClient> _logger;

    public OzonLoadProductIdentifiersClient(ILogger<OzonLoadProductIdentifiersClient> logger)
    {
        _logger = logger;
    }

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

            Thread.Sleep(1000); // Ozon API limit 1000 requests per minute
        }

        return result;
    }

    private async Task<Dictionary<string, OzonProductIdentifier>> LoadOzonIdentifiersAsync(OzonDto ozon,
        IEnumerable<KeyValuePair<string, long>> portion)
    {
        var request = new OzonProductInfoRequest
        {
            ProductIds = portion.Select(kv => kv.Value)
        };

        var httpResponse = await PostAsync(ozon, "v3/product/info/list", request);
        var body = await httpResponse.Content.ReadAsStringAsync();

        var response = body.FromJson<OzonProductInfoResult>();

        if (response is null)
        {
            var message = $"Ozon update product identifiers error." +
                          $"{Environment.NewLine}Can't deserialize body: {body}";

            throw new Exception(message);
        }

        _logger.LogInformation("Loaded product identifier list from Ozon for {Count} products", response.Items.Count());

        return response.Items.ToDictionary(i => i.OfferId, i => new OzonProductIdentifier
        {
            OzonId = i.Id.ToString(),
            FboSku = i.Sources.FirstOrDefault(x => x.Source.Equals("fbo", StringComparison.InvariantCultureIgnoreCase))
                ?.Sku.ToString(),
            FbsSku = i.Sources.FirstOrDefault(x => x.Source.Equals("sds", StringComparison.InvariantCultureIgnoreCase))
                ?.Sku.ToString()
        });
    }

    private async Task<Dictionary<string, long>> LoadOzonBaseIdentifiersAsync(OzonDto ozon)
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

    private async Task<(int, string)> LoadProductPageAsync(OzonDto ozon, IDictionary<string, long> result,
        string lastId)
    {
        var request = new OzonProductIdsRequest
        {
            LastId = lastId,
            Limit = ozon.Settings.LoadProductIdsPageSize
        };

        var httpResponse = await PostAsync(ozon, "v3/product/list", request);
        var body = await httpResponse.Content.ReadAsStringAsync();

        var response = body.FromJson<OzonProductIdsResponse>();

        if (response is null)
        {
            var message = $"Ozon update product ids error." +
                          $"{Environment.NewLine}Can't deserialize body: {body}";

            throw new Exception(message);
        }

        _logger.LogInformation("Loaded product base identifier list from Ozon for {Count} products.",
            response.Result.Items.Count);

        foreach (var item in response.Result.Items)
        {
            result.TryAdd(item.OfferId, item.ProductId);
        }

        return (response.Result.Total, response.Result.LastId);
    }
}
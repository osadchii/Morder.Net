using Infrastructure.Extensions;
using Infrastructure.Models.Marketplaces.Ozon;
using Integration.Ozon.Clients.LoadProducts.Messages;

namespace Integration.Ozon.Clients.LoadProducts;

public interface IOzonLoadProductIdsClient
{
    public Task<Dictionary<string, string>> LoadProductIdsAsync(OzonDto ozon);
}

public class OzonLoadProductIdsClient : BaseOzonClient, IOzonLoadProductIdsClient
{
    public async Task<Dictionary<string, string>> LoadProductIdsAsync(OzonDto ozon)
    {
        var result = new Dictionary<string, string>();
        var lastId = string.Empty;
        var currentPage = 0;
        var loaded = false;
        const int pageLimit = 500;

        while (!loaded)
        {
            (int, string) response = await LoadProductPageAsync(ozon, result, lastId);
            lastId = response.Item2;
            
            if (result.Count >= response.Item1 || currentPage++ > pageLimit)
            {
                loaded = true;
            }
        }

        return result;
    }

    private async Task<(int, string)> LoadProductPageAsync(OzonDto ozon, IDictionary<string, string> result, string lastId)
    {
        var request = new OzonProductIdsRequest
        {
            LastId = lastId,
            Limit = ozon.Settings.LoadProductIdsPageSize
        };

        HttpResponseMessage httpResponse = await PostAsync(ozon, "v1/product/list", request);
        var body = await httpResponse.Content.ReadAsStringAsync();

        var response = body.FromJson<OzonProductIdsResponse>();

        if (response is null)
        {
            var message = $"Ozon update product ids error." +
                          $"{Environment.NewLine}Can't deserialize body: {body}";

            throw new Exception(message);
        }

        foreach (OzonProductIdsResponseItem item in response.Result.Items)
        {
            result.TryAdd(item.OfferId, item.ProductId.ToString());
        }

        return (response.Result.Total, response.Result.LastId);
    }
}
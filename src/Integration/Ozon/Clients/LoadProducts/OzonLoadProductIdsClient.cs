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
        var currentPage = 1;
        var loaded = false;
        const int pageLimit = 500;

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
        var request = new OzonProductIdsRequest
        {
            Page = page,
            PageSize = ozon.Settings.LoadProductIdsPageSize
        };

        HttpResponseMessage httpResponse = await PostAsync(ozon, "v1/product/list", request);
        string body = await httpResponse.Content.ReadAsStringAsync();

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
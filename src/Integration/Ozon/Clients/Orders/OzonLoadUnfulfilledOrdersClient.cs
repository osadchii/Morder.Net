using Infrastructure.Extensions;
using Infrastructure.Models.Marketplaces.Ozon;
using Integration.Ozon.Clients.Orders.Messages;

namespace Integration.Ozon.Clients.Orders;

public interface IOzonLoadUnfulfilledOrdersClient
{
    Task<IEnumerable<Posting>> GetOrders(OzonDto ozonDto);
}

public class OzonLoadUnfulfilledOrdersClient : BaseOzonClient, IOzonLoadUnfulfilledOrdersClient
{
    public async Task<IEnumerable<Posting>> GetOrders(OzonDto ozonDto)
    {
        var result = new List<Posting>();
        var currentPage = 1;
        var loaded = false;
        const int pageLimit = 1000;

        while (!loaded)
        {
            int total = await LoadPostingsPortions(ozonDto, result, currentPage++);
            if (result.Count >= total || currentPage > pageLimit)
            {
                loaded = true;
            }
        }

        return result;
    }

    private async Task<int> LoadPostingsPortions(OzonDto ozon, List<Posting> postings, int currentPage)
    {
        var request = new GetUnfulfilledOrdersRequest()
        {
            Limit = ozon.Settings.LoadOrdersPageSize,
            Offset = (currentPage - 1) * ozon.Settings.LoadOrdersPageSize,
            Filter = new GetUnfulfilledOrdersFilter()
            {
                CutoffFrom = DateTime.UtcNow,
                CutoffTo = DateTime.UtcNow.AddDays(ozon.Settings.LoadOrdersDaysInterval)
            }
        };

        HttpResponseMessage httpResponse = await PostAsync(ozon, "v3/posting/fbs/unfulfilled/list", request);
        string body = await httpResponse.Content.ReadAsStringAsync();

        var response = body.FromJson<GetUnfulfilledOrdersResponse>();

        if (response is null)
        {
            string message = $"Ozon update orders error." +
                             $"{Environment.NewLine}Can't deserialize body: {body}";

            throw new Exception(message);
        }

        foreach (Posting item in response.Result.Postings)
        {
            postings.Add(item);
        }

        return response.Result.Count;
    }
}
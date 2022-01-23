using System.Collections.Concurrent;
using Infrastructure.Extensions;
using Infrastructure.Models.Marketplaces.Ozon;
using Integration.Ozon.Clients.Orders.Messages;

namespace Integration.Ozon.Clients.Orders;

public interface IOzonLoadOrderListClient
{
    Task<List<OzonPosting>> GetOrders(OzonDto ozonDto, DateTime startDate);
}

public class OzonLoadOrderListClient : BaseOzonClient, IOzonLoadOrderListClient
{
    public async Task<List<OzonPosting>> GetOrders(OzonDto ozonDto, DateTime startDate)
    {
        var result = new ConcurrentBag<OzonPosting>();

        DateTime currentDate = new DateTime(startDate.Year, startDate.Month, 1, 0, 0, 0).ToUtcTime();
        var months = new List<DateTime>();

        while (currentDate < DateTime.UtcNow)
        {
            currentDate = currentDate.AddMonths(1);
            months.Add(currentDate);
        }

        await Parallel.ForEachAsync(months, new ParallelOptions()
        {
            MaxDegreeOfParallelism = ozonDto.Settings.UpdateOrdersThreads
        }, async (time, _) => { await LoadPostingByMonth(ozonDto, result, time); });

        return result.ToList();
    }

    private static async Task LoadPostingByMonth(OzonDto ozonDto, ConcurrentBag<OzonPosting> postings,
        DateTime month)
    {
        var currentPage = 1;
        var loaded = false;
        const int pageLimit = 1000;

        while (!loaded)
        {
            loaded = await LoadPostingsPortions(ozonDto, postings, month, currentPage++) || currentPage > pageLimit;
        }
    }

    private static async Task<bool> LoadPostingsPortions(OzonDto ozon, ConcurrentBag<OzonPosting> postings,
        DateTime month, int currentPage)
    {
        var request = new GetOrderListRequest()
        {
            Limit = ozon.Settings.LoadOrdersPageSize,
            Offset = (currentPage - 1) * ozon.Settings.LoadOrdersPageSize,
            Filter = new GetOrderListFilter()
            {
                Since = month,
                To = month.AddMonths(1).AddMilliseconds(-1)
            }
        };

        HttpResponseMessage httpResponse = await PostAsync(ozon, "v3/posting/fbs/list", request);
        string body = await httpResponse.Content.ReadAsStringAsync();

        var response = body.FromJson<GetOrderListResponse>();

        if (response is null)
        {
            string message = $"Ozon load orders error." +
                             $"{Environment.NewLine}Can't deserialize body: {body}";

            throw new Exception(message);
        }

        foreach (OzonPosting posting in response.Result.Postings)
        {
            postings.Add(posting);
        }

        return !response.Result.Postings.Any();
    }
}
using System.Collections.Concurrent;
using Infrastructure.Extensions;
using Infrastructure.Models.Marketplaces.Ozon;
using Integration.Ozon.Clients.Orders.Messages;
using Microsoft.Extensions.Logging;

namespace Integration.Ozon.Clients.Orders;

public interface IOzonLoadOrderListClient
{
    Task<List<OzonPosting>> GetOrders(OzonDto ozonDto, DateTime startDate);
}

public class OzonLoadOrderListClient : BaseOzonClient, IOzonLoadOrderListClient
{
    private readonly ILogger<OzonLoadOrderListClient> _logger;
    private const int DaysInterval = 7;

    public OzonLoadOrderListClient(ILogger<OzonLoadOrderListClient> logger)
    {
        _logger = logger;
    }

    public async Task<List<OzonPosting>> GetOrders(OzonDto ozonDto, DateTime startDate)
    {
        var result = new ConcurrentBag<OzonPosting>();

        var currentDate = startDate;
        var intervals = new List<(DateTime Since, DateTime To)>();

        while (currentDate < DateTime.UtcNow.AddDays(DaysInterval * 2))
        {
            intervals.Add((Since: currentDate, To: currentDate.AddDays(DaysInterval)));
            currentDate = currentDate.AddDays(DaysInterval);
        }

        await Parallel.ForEachAsync(intervals, new ParallelOptions
        {
            MaxDegreeOfParallelism = ozonDto.Settings.UpdateOrdersThreads
        }, async (interval, _) => { await LoadPostingByInterval(ozonDto, result, interval); });

        return result.DistinctBy(p => p.PostingNumber).ToList();
    }

    private async Task LoadPostingByInterval(OzonDto ozonDto, ConcurrentBag<OzonPosting> postings,
        (DateTime Since, DateTime To) interval)
    {
        var currentPage = 1;
        var loaded = false;
        const int pageLimit = 1000;

        while (!loaded)
        {
            loaded = await LoadPostingsPortions(ozonDto, postings, interval, currentPage++) || currentPage > pageLimit;
        }
    }

    private async Task<bool> LoadPostingsPortions(OzonDto ozon, ConcurrentBag<OzonPosting> postings,
        (DateTime Since, DateTime To) interval, int currentPage)
    {
        var request = new GetOrderListRequest
        {
            Limit = ozon.Settings.LoadOrdersPageSize,
            Offset = (currentPage - 1) * ozon.Settings.LoadOrdersPageSize,
            Filter = new GetOrderListFilter
            {
                Since = interval.Since,
                To = interval.To
            }
        };

        HttpResponseMessage httpResponse;

        try
        {
            httpResponse = await PostAsync(ozon, "v3/posting/fbs/list", request);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Error while loading ozon order portion: {Message}", ex.Message);
            return true;
        }

        var body = await httpResponse.Content.ReadAsStringAsync();

        var response = body.FromJson<GetOrderListResponse>();

        if (response is null)
        {
            var message = $"Ozon load orders error." +
                          $"{Environment.NewLine}Can't deserialize body: {body}";

            throw new Exception(message);
        }

        foreach (var posting in response.Result.Postings)
        {
            postings.Add(posting);
        }

        return !response.Result.Postings.Any();
    }
}
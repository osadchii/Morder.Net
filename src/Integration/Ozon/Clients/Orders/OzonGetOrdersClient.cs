using System.Collections.Concurrent;
using Infrastructure.Extensions;
using Infrastructure.Models.Marketplaces.Ozon;
using Integration.Ozon.Clients.Orders.Messages;

namespace Integration.Ozon.Clients.Orders;

public interface IOzonGetOrdersClient
{
    Task<List<OzonPosting>> GetOrders(OzonDto ozonDto, IEnumerable<string> orderNumbers);
}

public class OzonGetOrdersClient : BaseOzonClient, IOzonGetOrdersClient
{
    public async Task<List<OzonPosting>> GetOrders(OzonDto ozonDto, IEnumerable<string> orderNumbers)
    {
        var result = new ConcurrentBag<OzonPosting>();

        await Parallel.ForEachAsync(orderNumbers, new ParallelOptions()
        {
            MaxDegreeOfParallelism = ozonDto.Settings.LoadOrdersThreads
        }, async (number, _) => { await LoadOrder(ozonDto, result, number); });

        return result.ToList();
    }

    private static async Task LoadOrder(OzonDto ozonDto, ConcurrentBag<OzonPosting> postings, string number)
    {
        var request = new GetOrderRequest()
        {
            PostingNumber = number
        };

        HttpResponseMessage httpResponse = await PostAsync(ozonDto, "v3/posting/fbs/get", request);
        string body = await httpResponse.Content.ReadAsStringAsync();

        var response = body.FromJson<GetOrderResponse>();

        if (response is null)
        {
            string message = $"Ozon update orders error." +
                             $"{Environment.NewLine}Can't deserialize body: {body}";

            throw new Exception(message);
        }

        postings.Add(response.Result);
    }
}
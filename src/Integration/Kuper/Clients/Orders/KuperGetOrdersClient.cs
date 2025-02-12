using System.Text;
using Infrastructure.Extensions;
using Infrastructure.Models.Marketplaces.Kuper;
using Integration.Kuper.Clients.Orders.Messages;
using Microsoft.Extensions.Logging;

namespace Integration.Kuper.Clients.Orders;

public interface IKuperOrdersClient
{
    Task<string> GetToken(KuperDto kuper, HttpClient httpClient);
    Task<List<OrderData>> GetOrders(KuperDto kuper, string token = null);
    Task<List<OrderData>> GetOrdersByStatus(KuperDto kuper, string status, string token = null);
    Task<List<OrderData>> GetOrdersByCreationDate(KuperDto kuper, DateTime from, DateTime to, string token = null);
    Task<List<OrderData>> GetOrdersByUpdatedDate(KuperDto kuper, DateTime from, DateTime to, string token = null);
    Task<List<OrderData>> GetOrdersByDeliveryDate(KuperDto kuper, DateTime from, DateTime to, string token = null);
    Task<List<OrderData>> GetOrdersByPaymentStatus(KuperDto kuper, string paymentStatus, string token = null);
    Task<List<OrderData>> GetOrdersByStoreId(KuperDto kuper, string storeId, string token = null);

    Task<OrdersPaginationResult> GetOrdersWIthPagination(KuperDto kuper, int? maxPageSize = null,
        string nextPageToken = null,
        string token = null);

    Task SendOrderNotification(KuperDto kuper, KuperOrderNotification notification, string token = null);
    Task<KuperOrderState> GetOrderState(KuperDto kuper, string orderId, string token = null);
    Task<OrderData> GetOrder(KuperDto kuper, string orderId, string token = null);
}

public class KuperOrdersClient : KuperClientBase, IKuperOrdersClient
{
    public KuperOrdersClient(ILogger<KuperOrdersClient> logger)
    {
        _logger = logger;
    }

    private readonly ILogger<KuperOrdersClient> _logger;

    public async Task<List<OrderData>> GetOrders(KuperDto kuper, string token = null)
    {
        var response = await GetAsync(kuper, "/ofm/api/v1/shipments", token);
        var content = await response.Content.ReadAsStringAsync();
        _logger.LogInformation(content);

        var orders = content.FromJson<KuperOrdersMessage>();

        return orders.Data;
    }

    public async Task<List<OrderData>> GetOrdersByStatus(KuperDto kuper, string status, string token = null)
    {
        var response = await GetAsync(kuper, "/ofm/api/v1/shipments?filters[state]=" + status, token);
        var orders = await response.Content.ReadAsObject<KuperOrdersMessage>();

        return orders.Data;
    }

    public async Task<List<OrderData>> GetOrdersByCreationDate(KuperDto kuper, DateTime from, DateTime to,
        string token = null)
    {
        var response = await GetAsync(kuper,
            "/ofm/api/v1/shipments?filters[createdFrom]=" + from.ToString("yyyy-MM-ddTHH:mm:ss") +
            "&filters[createdTo]=" +
            to.ToString("yyyy-MM-ddTHH:mm:ss"), token);
        var orders = await response.Content.ReadAsObject<KuperOrdersMessage>();

        return orders.Data;
    }

    public async Task<List<OrderData>> GetOrdersByUpdatedDate(KuperDto kuper, DateTime from, DateTime to,
        string token = null)
    {
        var response = await GetAsync(kuper,
            "/ofm/api/v1/shipments?filters[updatedFrom]=" + from.ToString("yyyy-MM-ddTHH:mm:ss") +
            "&filters[updatedTo]=" +
            to.ToString("yyyy-MM-ddTHH:mm:ss"), token);
        var orders = await response.Content.ReadAsObject<KuperOrdersMessage>();

        return orders.Data;
    }

    public async Task<List<OrderData>> GetOrdersByDeliveryDate(KuperDto kuper, DateTime from, DateTime to,
        string token = null)
    {
        var response = await GetAsync(kuper,
            "/ofm/api/v1/shipments?filters[deliveryFrom]=" + from.ToString("yyyy-MM-ddTHH:mm:ss") +
            "&filters[deliveryTo]=" +
            to.ToString("yyyy-MM-ddTHH:mm:ss"), token);
        var orders = await response.Content.ReadAsObject<KuperOrdersMessage>();

        return orders.Data;
    }

    public async Task<List<OrderData>> GetOrdersByPaymentStatus(KuperDto kuper, string paymentStatus,
        string token = null)
    {
        var response = await GetAsync(kuper, "/ofm/api/v1/shipments?filters[paymentState]=" + paymentStatus, token);
        var orders = await response.Content.ReadAsObject<KuperOrdersMessage>();

        return orders.Data;
    }

    public async Task<List<OrderData>> GetOrdersByStoreId(KuperDto kuper, string storeId, string token = null)
    {
        var response = await GetAsync(kuper, "/ofm/api/v1/shipments?filters[storeId]=" + storeId, token);
        var orders = await response.Content.ReadAsObject<KuperOrdersMessage>();

        return orders.Data;
    }

    public async Task<OrdersPaginationResult> GetOrdersWIthPagination(KuperDto kuper, int? maxPageSize = null,
        string nextPageToken = null, string token = null)
    {
        var sb = new StringBuilder("/ofm/api/v1/shipments?");

        if (maxPageSize.HasValue)
        {
            sb.Append("maxPageSize=" + maxPageSize);
        }
        else if (!nextPageToken.IsNullOrEmpty())
        {
            sb.Append("nextPageToken=" + nextPageToken);
        }
        else
        {
            throw new InvalidOperationException("maxPageSize or nextPageToken must be set");
        }

        var response = await GetAsync(kuper, sb.ToString(), token);
        var orders = await response.Content.ReadAsObject<KuperOrdersMessage>();

        return new OrdersPaginationResult(orders.Data, orders.NextPageToken);
    }

    public Task SendOrderNotification(KuperDto kuper, KuperOrderNotification notification, string token = null)
    {
        return PostAsync(kuper, "/ofm/api/v1/notifications", notification, token);
    }

    public async Task<KuperOrderState> GetOrderState(KuperDto kuper, string orderId, string token = null)
    {
        var response = await GetAsync(kuper, $"/ofm/api/v1/shipments/{orderId}/state", token);
        var order = await response.Content.ReadAsObject<KuperOrderState>();

        return order;
    }

    public async Task<OrderData> GetOrder(KuperDto kuper, string orderId, string token = null)
    {
        var response = await GetAsync(kuper, $"/ofm/api/v1/shipments/{orderId}", token);
        var order = await response.Content.ReadAsObject<KuperOrderMessage>();

        return order.Data;
    }
}

public record OrdersPaginationResult(List<OrderData> Data, string NextPageToken);
using Infrastructure.Extensions;
using Infrastructure.Models.Marketplaces.Kuper;

namespace Integration.Kuper.Clients.Orders;

public interface IKuperGetOrdersClient
{
    Task<string> GetToken(KuperDto kuper, HttpClient httpClient);
    Task<List<OrderData>> GetOrders(KuperDto kuper, string token = null);
    Task<List<OrderData>> GetOrdersByStatus(KuperDto kuper, string status, string token = null);
    Task<List<OrderData>> GetOrdersByCreationDate(KuperDto kuper, DateTime from, DateTime to, string token = null);
    Task<List<OrderData>> GetOrdersByUpdatedDate(KuperDto kuper, DateTime from, DateTime to, string token = null);
    Task<List<OrderData>> GetOrdersByDeliveryDate(KuperDto kuper, DateTime from, DateTime to, string token = null);
    Task<List<OrderData>> GetOrdersByPaymentStatus(KuperDto kuper, string paymentStatus, string token = null);
    Task<List<OrderData>> GetOrdersByStoreId(KuperDto kuper, string storeId, string token = null);
}

public class KuperGetOrdersClient : KuperClientBase, IKuperGetOrdersClient
{
    public async Task<List<OrderData>> GetOrders(KuperDto kuper, string token = null)
    {
        var response = await GetAsync(kuper, "/v1/shipments", token);
        var orders = await response.Content.ReadAsObject<KuperOrdersMessage>();

        return orders.Data;
    }

    public async Task<List<OrderData>> GetOrdersByStatus(KuperDto kuper, string status, string token = null)
    {
        var response = await GetAsync(kuper, "/v1/shipments?filters[status]=" + status, token);
        var orders = await response.Content.ReadAsObject<KuperOrdersMessage>();

        return orders.Data;
    }

    public async Task<List<OrderData>> GetOrdersByCreationDate(KuperDto kuper, DateTime from, DateTime to,
        string token = null)
    {
        var response = await GetAsync(kuper,
            "/v1/shipments?filters[createdFrom]=" + from.ToString("yyyy-MM-ddTHH:mm:ss") + "&filters[createdTo]=" +
            to.ToString("yyyy-MM-ddTHH:mm:ss"), token);
        var orders = await response.Content.ReadAsObject<KuperOrdersMessage>();

        return orders.Data;
    }

    public async Task<List<OrderData>> GetOrdersByUpdatedDate(KuperDto kuper, DateTime from, DateTime to,
        string token = null)
    {
        var response = await GetAsync(kuper,
            "/v1/shipments?filters[updatedFrom]=" + from.ToString("yyyy-MM-ddTHH:mm:ss") + "&filters[updatedTo]=" +
            to.ToString("yyyy-MM-ddTHH:mm:ss"), token);
        var orders = await response.Content.ReadAsObject<KuperOrdersMessage>();

        return orders.Data;
    }

    public async Task<List<OrderData>> GetOrdersByDeliveryDate(KuperDto kuper, DateTime from, DateTime to,
        string token = null)
    {
        var response = await GetAsync(kuper,
            "/v1/shipments?filters[deliveryFrom]=" + from.ToString("yyyy-MM-ddTHH:mm:ss") + "&filters[deliveryTo]=" +
            to.ToString("yyyy-MM-ddTHH:mm:ss"), token);
        var orders = await response.Content.ReadAsObject<KuperOrdersMessage>();

        return orders.Data;
    }

    public async Task<List<OrderData>> GetOrdersByPaymentStatus(KuperDto kuper, string paymentStatus,
        string token = null)
    {
        var response = await GetAsync(kuper, "/v1/shipments?filters[paymentState]=" + paymentStatus, token);
        var orders = await response.Content.ReadAsObject<KuperOrdersMessage>();

        return orders.Data;
    }

    public async Task<List<OrderData>> GetOrdersByStoreId(KuperDto kuper, string storeId, string token = null)
    {
        var response = await GetAsync(kuper, "/v1/shipments?filters[storeId]=" + storeId, token);
        var orders = await response.Content.ReadAsObject<KuperOrdersMessage>();

        return orders.Data;
    }
}
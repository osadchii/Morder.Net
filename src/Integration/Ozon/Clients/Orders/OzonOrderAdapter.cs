using Infrastructure.Cache.Interfaces;
using Infrastructure.Extensions;
using Infrastructure.MediatR.Orders.Marketplace.Common.Commands;
using Infrastructure.Models.Marketplaces.Ozon;
using Infrastructure.Models.Orders;
using Integration.Ozon.Clients.Orders.Messages;

namespace Integration.Ozon.Clients.Orders;

public interface IOzonOrderAdapter
{
    public Task<IEnumerable<CreateOrderRequest>> CreateOrderRequests(OzonDto ozon,
        GetOrderListPosting[] postings);
}

public class OzonOrderAdapter : IOzonOrderAdapter
{
    private readonly IProductCache _productCache;
    private const string Customer = "Ozon Customer";

    public OzonOrderAdapter(IProductCache productCache)
    {
        _productCache = productCache;
    }

    public async Task<IEnumerable<CreateOrderRequest>> CreateOrderRequests(OzonDto ozon, GetOrderListPosting[] postings)
    {
        List<string> requestArticuls = postings
            .SelectMany(p => p.Products.Select(product => product.OfferId))
            .Distinct()
            .ToList();

        Dictionary<string, int> products = await _productCache.GetProductIdsByArticul(requestArticuls);

        return postings
            .Select(p =>
            {
                return new CreateOrderRequest()
                {
                    Customer = p.Custromer?.Name ?? Customer,
                    Date = p.InProcessAt.ToUtcTime(),
                    Number = p.PostingNumber,
                    Status = p.Status switch
                    {
                        "acceptance_in_progress" => OrderStatus.Shipped,
                        "awaiting_approve" => OrderStatus.Created,
                        "awaiting_packaging" => OrderStatus.Reserved,
                        "awaiting_deliver" => OrderStatus.Packed,
                        "arbitration" => OrderStatus.Shipped,
                        "client_arbitration" => OrderStatus.Shipped,
                        "delivering" => OrderStatus.Shipped,
                        "driver_pickup" => OrderStatus.Shipped,
                        "delivered" => OrderStatus.Finished,
                        "cancelled" => OrderStatus.Canceled,
                        "not_accepted" => OrderStatus.Shipped,
                        _ => OrderStatus.Created
                    },
                    ExternalId = Guid.NewGuid(),
                    MarketplaceId = ozon.Id,
                    Archived = ozon.Settings.LoadOrdersAsArchived,
                    ShippingDate = p.ShipmentDate.ToUtcTime(),
                    Items = p.Products.Select(i => new CreateOrderItem()
                    {
                        Count = i.Quantity,
                        Price = decimal.Parse(i.Price),
                        Sum = i.Quantity * decimal.Parse(i.Price),
                        ProductId = products[i.OfferId]
                    })
                };
            });
    }
}
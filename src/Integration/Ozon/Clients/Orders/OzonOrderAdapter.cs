using Infrastructure.Cache.Interfaces;
using Infrastructure.Extensions;
using Infrastructure.MediatR.Orders.Marketplace.Common.Commands;
using Infrastructure.Models.Marketplaces.Ozon;
using Integration.Ozon.Clients.Orders.Messages;

namespace Integration.Ozon.Clients.Orders;

public interface IOzonOrderAdapter
{
    public Task<IEnumerable<CreateOrderRequest>> CreateOrderRequests(OzonDto ozon,
        OzonPosting[] postings);
}

public class OzonOrderAdapter : IOzonOrderAdapter
{
    private readonly IProductCache _productCache;
    private const string Customer = "Ozon Customer";

    public OzonOrderAdapter(IProductCache productCache)
    {
        _productCache = productCache;
    }

    public async Task<IEnumerable<CreateOrderRequest>> CreateOrderRequests(OzonDto ozon, OzonPosting[] postings)
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
                    Status = StatusConverter.OzonStatusToOrderStatus(p.Status),
                    ExternalId = Guid.NewGuid(),
                    MarketplaceId = ozon.Id,
                    Archived = ozon.Settings.LoadOrdersAsArchived,
                    ShippingDate = p.ShipmentDate.ToUtcTime(),
                    ExpressOrder = p.IsExpress,
                    TrackNumber = p.TrackingNumber,
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
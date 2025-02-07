using Infrastructure.Cache.Interfaces;
using Infrastructure.MediatR.Orders.Marketplace.Common.Commands;
using Infrastructure.Models.Marketplaces.Kuper;

namespace Integration.Kuper.Clients.Orders;

public interface IKuperOrderAdapter
{
    Task<IEnumerable<CreateOrderRequest>> CreateOrderRequests(KuperDto kuper, OrderData[] postings);
}

public class KuperOrderAdapter : IKuperOrderAdapter
{
    private readonly IProductCache _productCache;

    public KuperOrderAdapter(IProductCache productCache)
    {
        _productCache = productCache;
    }

    private const string Customer = "Kuper Customer";

    public async Task<IEnumerable<CreateOrderRequest>> CreateOrderRequests(KuperDto kuper, OrderData[] postings)
    {
        var requestArticuls = postings
            .SelectMany(p => p.Positions.Select(product => product.Id))
            .Distinct()
            .ToList();

#if DEBUG
        var products = await _productCache.GetProductIdsByArticul(requestArticuls, true);
#else
        Dictionary<string, int> products = await _productCache.GetProductIdsByArticul(requestArticuls);
#endif
        
        return postings
            .Where(p => p.Positions.All(i => products.ContainsKey(i.Id)))
            .Select(p =>
            {
                return new CreateOrderRequest
                {
                    Customer = p.Customer?.Name ?? Customer,
                    Date = DateTime.UtcNow,
                    Number = p.OriginalOrderId,
                    Status = StatusConverter.OzonStatusToOrderStatus(p.State),
                    ExternalId = Guid.NewGuid(),
                    MarketplaceId = kuper.Id,
                    Archived = kuper.Settings.LoadOrdersAsArchived == true,
                    ShippingDate = p.Delivery.ExpectedTo,
                    ExpressOrder = true,
                    Items = p.Positions.Select(i => new CreateOrderItem
                    {
                        Count = i.Quantity,
                        Price = decimal.Parse(i.Price),
                        Sum = i.Quantity * decimal.Parse(i.Price),
                        ProductId = products[i.Id]
                    })
                };
            });
    }
}
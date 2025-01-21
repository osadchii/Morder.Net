using System.Globalization;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Products;
using Newtonsoft.Json;

namespace Integration.Kuper.Feeds;

public class KuperStockFeed : KuperFeed<KuperStockFeed.Item>
{
    public class Item
    {
        [JsonProperty("offer_id")] public string OfferId { get; set; }
        [JsonProperty("outlet_id")] public string OutletId { get; set; }
        [JsonProperty("stock")] public string Stock { get; set; }
    }

    public static KuperStockFeed Build(IEnumerable<Product> products, MarketplaceProductData data,
        string warehouseId)
    {
        var stocks = products
            .Select(x =>
            {
                var price = data.GetProductPrice(x);
                return new KuperStockFeed.Item
                {
                    OfferId = x.Articul,
                    Stock = data.GetProductStock(x, price).ToString(CultureInfo.InvariantCulture),
                    OutletId = warehouseId
                };
            })
            .ToArray();

        var feed = new KuperStockFeed
        {
            Data = stocks
        };
        return feed;
    }
}
using System.Globalization;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Products;
using Integration.Kuper.Extensions;
using Newtonsoft.Json;

namespace Integration.Kuper.Feeds;

public class KuperPriceFeed : KuperFeed<KuperPriceFeed.Item>
{
    public class Item
    {
        [JsonProperty("offer_id")]
        public string OfferId { get; set; }
        [JsonProperty("outlet_id")]
        public string OutletId { get; set; }
        [JsonProperty("price_category")]
        public string PriceCategory { get; set; }
        [JsonProperty("price_type")]
        public string PriceType { get; set; }
        [JsonProperty("price")]
        public PriceValue Price { get; set; }
        [JsonProperty("vat")]
        public string Vat { get; set; }
        
        public class PriceValue
        {
            [JsonProperty("currency")]
            public string Currency { get; set; }
            [JsonProperty("amount")]
            public string Amount { get; set; }
        }
    }

    public static KuperPriceFeed Build(IEnumerable<Product> products, MarketplaceProductData data,
        string warehouseId)
    {
        var prices = products
            .Select(x => new Item
            {
                OfferId = x.Articul,
                OutletId = warehouseId,
                PriceType = "BASE",
                PriceCategory = "ONLINE",
                Vat = x.Vat.ConvertToString(),
                Price = new Item.PriceValue
                {
                    Currency = "RUB",
                    Amount = data.GetProductPrice(x).ToString(CultureInfo.InvariantCulture)
                }
            })
            .ToArray();

        var feed = new KuperPriceFeed
        {
            Data = prices
        };

        return feed;
    }
}
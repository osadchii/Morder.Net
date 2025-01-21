using System.Globalization;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Products;
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
            .Select(x =>
            {
                return new KuperPriceFeed.Item
                {
                    OfferId = x.Articul,
                    OutletId = warehouseId,
                    PriceType = "BASE",
                    PriceCategory = "ONLINE",
                    Vat = x.Vat switch
                    {
                        Vat.No_vat => "NO_VAT",
                        Vat.Vat_0 => "VAT0",
                        Vat.Vat_10 => "VAT10",
                        Vat.Vat_20 => "VAT20",
                        _ => "NO_VAT"
                    },
                    Price = new KuperPriceFeed.Item.PriceValue
                    {
                        Currency = "RUB",
                        Amount = data.GetProductPrice(x).ToString(CultureInfo.InvariantCulture)
                    }
                };
            })
            .ToArray();

        var feed = new KuperPriceFeed
        {
            Data = prices
        };

        return feed;
    }
}
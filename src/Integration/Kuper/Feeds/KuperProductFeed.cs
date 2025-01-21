using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Products;
using Infrastructure.Services.Marketplaces;
using Integration.Kuper.Extensions;
using Newtonsoft.Json;

namespace Integration.Kuper.Feeds;

public class KuperProductFeed : KuperFeed<KuperProductFeed.Item>
{
    public class Item
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("categories_ids")] public string[] CategoryIds { get; set; }
        [JsonProperty("status")] public string Status { get; set; }
        [JsonProperty("pack_type")] public string PackType { get; set; }
        [JsonProperty("barcodes")] public string[] Barcodes { get; set; }
        [JsonProperty("images")] public Image[] Images { get; set; }
        [JsonProperty("attributes")] public Attribute[] Attributes { get; set; }

        public class Image
        {
            [JsonProperty("url")] public string Url { get; set; }
            [JsonProperty("name")] public string Name { get; set; }
        }

        public class Attribute
        {
            [JsonProperty("attribute")] public string Name { get; set; }
            [JsonProperty("values")] public string[] Values { get; set; }
        }
    }

    public static KuperProductFeed Build(IEnumerable<Product> products, MarketplaceProductData data,
        IProductImageService productImageService)
    {
        var feed = new KuperProductFeed
        {
            Data = products
                .Select(x => x.ToKuperProduct(data, productImageService))
                .ToArray()
        };
        return feed;
    }

    public static class KuperFeedAttributes
    {
        public const string Brand = "brand";
        public const string Manufacturer = "manufacturer";
        public const string Country = "country";
        public const string VendorCode = "vendorCode";
        public const string IsAlcohol = "is_alcohol";
        public const string IsExcisable = "is_excisable";
        public const string IsOwnBrand = "is_own_brand";
        public const string IsPrivateLabel = "is_private_label";
        public const string Height = "height";
        public const string Width = "width";
        public const string Length = "length";
        public const string VolumeNet = "volume_netto";
        public const string WeightNet = "weight_netto";
        public const string VolumeNetUnit = "volume_netto_unit";
        public const string WeightNetUnit = "weight_netto_unit";
    }
}
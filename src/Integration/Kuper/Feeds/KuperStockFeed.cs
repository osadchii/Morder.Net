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
}
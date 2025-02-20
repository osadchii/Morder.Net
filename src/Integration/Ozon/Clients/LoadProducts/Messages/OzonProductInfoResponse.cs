using Newtonsoft.Json;

namespace Integration.Ozon.Clients.LoadProducts.Messages;

public class OzonProductInfoResponse
{
    [JsonProperty("result")] public OzonProductInfoResult Result { get; set; } = null!;

    [JsonIgnore] public IEnumerable<OzonProductInfoItem> Items => Result.Items;
}

public class OzonProductInfoResult
{
    [JsonProperty("items")]
    public IEnumerable<OzonProductInfoItem> Items { get; set; } = null!;
}

public class OzonProductInfoItem
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("offer_id")] public string OfferId { get; set; } = null!;
    
    [JsonProperty("sources")] public OzonProductInfoItemSource[] Sources { get; set; } = null!;
}

public class OzonProductInfoItemSource
{
    [JsonProperty("sku")]
    public long Sku { get; set; }
    
    [JsonProperty("source")]
    public string Source { get; set; }
}
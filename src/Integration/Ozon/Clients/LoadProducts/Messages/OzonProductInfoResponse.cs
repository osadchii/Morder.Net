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

    [JsonProperty("fbs_sku")]
    public long FbsSku { get; set; }

    [JsonProperty("fbo_sku")]
    public long FboSku { get; set; }
}
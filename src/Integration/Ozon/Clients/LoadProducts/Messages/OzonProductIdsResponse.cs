using Newtonsoft.Json;

namespace Integration.Ozon.Clients.LoadProducts.Messages;

public class OzonProductIdsResponse
{
    [JsonProperty("result")] public OzonProductIdsResponseResult Result { get; set; } = null!;
}

public class OzonProductIdsResponseResult
{
    [JsonProperty("items")] public List<OzonProductIdsResponseItem> Items { get; set; } = null!;

    [JsonProperty("total")] public int Total { get; set; }

    [JsonProperty("last_id")] public string LastId { get; set; } = null!;
}

public class OzonProductIdsResponseItem
{
    [JsonProperty("product_id")] public int ProductId { get; set; }

    [JsonProperty("offer_id")] public string OfferId { get; set; } = null!;
}
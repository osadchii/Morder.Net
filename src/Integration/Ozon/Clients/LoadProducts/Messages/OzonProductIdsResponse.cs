using Newtonsoft.Json;

namespace Integration.Ozon.Clients.LoadProducts.Messages;

public class OzonProductIdsResponse
{
    [JsonProperty("result")] public OzonProductIdsResponseResult Result { get; set; }
}

public class OzonProductIdsResponseResult
{
    [JsonProperty("items")] public List<OzonProductIdsResponseItem> Items { get; set; }

    [JsonProperty("total")] public int Total { get; set; }
}

public class OzonProductIdsResponseItem
{
    [JsonProperty("product_id")] public int ProductId { get; set; }

    [JsonProperty("offer_id")] public string OfferId { get; set; }
}
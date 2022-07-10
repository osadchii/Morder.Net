using Newtonsoft.Json;

namespace Integration.Ozon.Clients.LoadProducts.Messages;

public class OzonProductInfoRequest
{
    [JsonProperty("product_id")] public IEnumerable<long> ProductIds { get; set; } = null!;
}
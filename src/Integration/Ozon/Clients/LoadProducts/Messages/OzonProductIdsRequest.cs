using Newtonsoft.Json;

namespace Integration.Ozon.Clients.LoadProducts.Messages;

public record OzonProductIdsRequest
{
    [JsonProperty("last_id")] public string LastId { get; set; }

    [JsonProperty("limit")] public int Limit { get; set; }
}
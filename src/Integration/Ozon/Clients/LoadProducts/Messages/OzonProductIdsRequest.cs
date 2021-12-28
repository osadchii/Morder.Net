using Newtonsoft.Json;

namespace Integration.Ozon.Clients.LoadProducts.Messages;

public record OzonProductIdsRequest
{
    [JsonProperty("page")] public int Page { get; set; }

    [JsonProperty("page_size")] public int PageSize { get; set; }
}
using Newtonsoft.Json;

namespace Integration.Ozon.Clients.Orders.Messages;

public class GetOrderListResponse
{
    [JsonProperty("has_next")] public bool HasNext { get; set; }

    [JsonProperty("result")] public GetOrderListResult Result { get; set; }
}

public class GetOrderListResult
{
    [JsonProperty("postings")] public IEnumerable<OzonPosting> Postings { get; set; }
}
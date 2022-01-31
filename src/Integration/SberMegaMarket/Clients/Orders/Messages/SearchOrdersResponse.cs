using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Clients.Orders.Messages;

public class SearchOrdersResponse
{
    [JsonProperty("success")] public int Success { get; set; }

    [JsonProperty("data")] public SearchOrdersResponseData Data { get; set; } = null!;

    public class SearchOrdersResponseData
    {
        [JsonProperty("shipments")] public IEnumerable<string> Shipments { get; set; } = null!;
    }
}
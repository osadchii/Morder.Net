using Newtonsoft.Json;

namespace Integration.Ozon.Clients.Orders.Messages;

public class GetOrderListRequest
{
    [JsonProperty("dir")] public string Dir => "ASC";

    [JsonProperty("limit")] public int Limit { get; set; }

    [JsonProperty("offset")] public int Offset { get; set; }

    [JsonProperty("filter")] public GetOrderListFilter Filter { get; set; } = null!;
}

public class GetOrderListFilter
{
    [JsonProperty("since")] public DateTime Since { get; set; }

    [JsonProperty("to")] public DateTime To { get; set; }
}
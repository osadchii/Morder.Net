using Newtonsoft.Json;

namespace Integration.Ozon.Clients.Orders.Messages;

public class GetUnfulfilledOrdersRequest
{
    [JsonProperty("dir")] public string Dir => "asc";

    [JsonProperty("limit")] public int Limit { get; set; }

    [JsonProperty("offset")] public int Offset { get; set; }

    [JsonProperty("filter")] public GetUnfulfilledOrdersFilter Filter { get; set; }
}

public class GetUnfulfilledOrdersFilter
{
    [JsonProperty("cutoff_from")] public DateTime CutoffFrom { get; set; }

    [JsonProperty("cutoff_to")] public DateTime CutoffTo { get; set; }
}
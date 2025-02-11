using Newtonsoft.Json;

namespace Integration.Kuper.Clients.Orders.Messages;

public class KuperOrderMessage
{
    [JsonProperty("data")] public OrderData Data { get; set; }

    [JsonProperty("status")] public string Status { get; set; }
}
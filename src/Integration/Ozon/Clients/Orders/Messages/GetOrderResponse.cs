using Newtonsoft.Json;

namespace Integration.Ozon.Clients.Orders.Messages;

public class GetOrderResponse
{
    [JsonProperty("result")] public OzonPosting Result { get; set; }
}
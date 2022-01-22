using Newtonsoft.Json;

namespace Integration.Ozon.Clients.Orders.Messages;

public class GetOrderRequest
{
    [JsonProperty("posting_number")] public string PostingNumber { get; set; }
}
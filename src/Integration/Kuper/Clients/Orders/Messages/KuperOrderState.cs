using Newtonsoft.Json;

namespace Integration.Kuper.Clients.Orders.Messages;

public class KuperOrderState
{
    [JsonProperty("status")]
    public string Status { get; set; }
    
    [JsonProperty("data")]
    public KuperOrderStateData Data { get; set; }
}

public class KuperOrderStateData
{
    [JsonProperty("state")]
    public string State { get; set; }
    
    [JsonProperty("paymentState")]
    public string PaymentState { get; set; }
}
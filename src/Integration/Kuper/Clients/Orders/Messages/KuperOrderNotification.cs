using Newtonsoft.Json;

namespace Integration.Kuper.Clients.Orders.Messages;

public class KuperOrderNotification
{
    [JsonProperty("event")]
    public Event Event { get; set; }
}

public class Event
{
    [JsonProperty("type")]
    public string Type { get; set; }
    [JsonProperty("payload")]
    public Payload Payload { get; set; }
}

public class Payload
{
    [JsonProperty("order_id")]
    public string OrderId { get; set; }
    [JsonProperty("orderUUID")]
    public string OrderUuid { get; set; }
    [JsonProperty("handoutCode")]
    public string HandoutCode { get; set; }
    [JsonProperty("shortNumber")]
    public string ShortNumber { get; set; }
    [JsonProperty("estimated_assembly_at")]
    public DateTime? EstimatedAssemblyAt { get; set; }
    [JsonProperty("number")]
    public string Number { get; set; }
    [JsonProperty("order")]
    public Order Order { get; set; }
}

public class Order
{
    [JsonProperty("originalOrderId")]
    public string OriginalOrderId { get; set; }

    [JsonProperty("positions")]
    public OrderPosition[] Positions { get; set; }
}

public class OrderPosition
{
    
    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("quantity")]
    public int Quantity { get; set; }
}
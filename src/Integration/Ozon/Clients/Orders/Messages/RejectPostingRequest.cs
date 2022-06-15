using Newtonsoft.Json;

namespace Integration.Ozon.Clients.Orders.Messages;

public class RejectPostingRequest
{
    [JsonProperty("cancel_reason_id")] public int CancelReasonId { get; set; } = 352;

    [JsonProperty("cancel_reason_message")]
    public string CancelReasonMessage { get; set; } = "Product is out of stock";

    [JsonProperty("posting_number")] public string PostingNumber { get; set; } = null!;

    [JsonProperty("items")] public IEnumerable<RejectPostingItem> Items { get; set; } = null!;
}

public class RejectPostingItem
{
    [JsonProperty("sku")]
    public int Sku { get; set; }
    
    [JsonProperty("quantity")]
    public int Quantity { get; set; }
}

public class RejectPostingResponse
{
    [JsonProperty("result")] public bool Result { get; set; }
}
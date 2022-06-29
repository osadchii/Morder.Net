using Newtonsoft.Json;

namespace Integration.Ozon.Clients.Orders.Messages;

public class OzonPosting
{
    [JsonProperty("posting_number")] public string PostingNumber { get; set; } = null!;

    [JsonProperty("status")] public string Status { get; set; } = null!;

    [JsonProperty("in_process_at")] public DateTime InProcessAt { get; set; }

    [JsonProperty("shipment_date")] public DateTime ShipmentDate { get; set; }

    [JsonProperty("is_express")] public bool IsExpress { get; set; }

    [JsonProperty("tracking_number")] public string TrackingNumber { get; set; } = null!;

    [JsonProperty("products")] public IEnumerable<OzonPostingProduct> Products { get; set; } = null!;

    [JsonProperty("customer")] public OzonPostingCustomer? Customer { get; set; }
}

public class OzonPostingProduct
{
    [JsonProperty("quantity")] public int Quantity { get; set; }

    [JsonProperty("offer_id")] public string OfferId { get; set; } = null!;

    [JsonProperty("price")] public string Price { get; set; } = null!;
}

public class OzonPostingCustomer
{
    [JsonProperty("address")] public OzonPostingCustomerAddress? Address { get; set; }

    [JsonProperty("name")] public string Name { get; set; } = null!;

    [JsonProperty("phone")] public string Phone { get; set; } = null!;
}

public class OzonPostingCustomerAddress
{
    [JsonProperty("address_tail")] public string AddressTail { get; set; } = null!;
}
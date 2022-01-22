using Newtonsoft.Json;

namespace Integration.Ozon.Clients.Orders.Messages;

public class GetUnfulfilledOrdersResponse
{
    [JsonProperty("result")] public GetUnfulfilledOrdersResult GetUnfulfilledOrdersResult { get; set; }
}

public class GetUnfulfilledOrdersResult
{
    [JsonProperty("count")] public int Count { get; set; }

    [JsonProperty("postings")] public IEnumerable<GetUnfulfilledOrdersPosting> Postings { get; set; }
}

public class GetUnfulfilledOrdersPosting
{
    [JsonProperty("customer")] public GetUnfulfilledOrdersPostingCustomer Customer { get; set; }

    [JsonProperty("in_process_at")] public DateTime InProcessAt { get; set; }

    [JsonProperty("shipment_date")] public DateTime ShipmentDate { get; set; }

    [JsonProperty("is_express")] public bool IsExpress { get; set; }

    [JsonProperty("posting_number")] public string PostingNumber { get; set; }

    [JsonProperty("status")] public string Status { get; set; }

    [JsonProperty("tracking_number")] public string TrackingNumber { get; set; }

    [JsonProperty("products")] public IEnumerable<GetUnfulfilledOrdersPostingProduct> Products { get; set; }
}

public class GetUnfulfilledOrdersPostingProduct
{
    [JsonProperty("offer_id")] public string OfferId { get; set; }

    [JsonProperty("price")] public string Price { get; set; }

    [JsonProperty("quantity")] public int Quantity { get; set; }
}

public class GetUnfulfilledOrdersPostingCustomer
{
    [JsonProperty("address")] public GetUnfulfilledOrdersPostingCustomerAddress Address { get; set; }

    [JsonProperty("name")] public string Name { get; set; }

    [JsonProperty("phone")] public string Phone { get; set; }
}

public class GetUnfulfilledOrdersPostingCustomerAddress
{
    [JsonProperty("address_tail")] public string AddressTail { get; set; }
}
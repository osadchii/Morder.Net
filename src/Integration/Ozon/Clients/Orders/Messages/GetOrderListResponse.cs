using Newtonsoft.Json;

namespace Integration.Ozon.Clients.Orders.Messages;

public class GetOrderListResponse
{
    [JsonProperty("has_next")] public bool HasNext { get; set; }

    [JsonProperty("result")] public GetOrderListResult Result { get; set; }
}

public class GetOrderListResult
{
    [JsonProperty("postings")] public IEnumerable<GetOrderListPosting> Postings { get; set; }
}

public class GetOrderListPosting
{
    [JsonProperty("posting_number")] public string PostingNumber { get; set; }

    [JsonProperty("status")] public string Status { get; set; }

    [JsonProperty("in_process_at")] public DateTime InProcessAt { get; set; }

    [JsonProperty("shipment_date")] public DateTime ShipmentDate { get; set; }

    [JsonProperty("is_express")] public bool IsExpress { get; set; }

    [JsonProperty("tracking_number")] public string TrackingNumber { get; set; }

    [JsonProperty("products")] public IEnumerable<GetOrderListPostingProduct> Products { get; set; }

    [JsonProperty("customer")] public GetOrderListPostingCustomer Custromer { get; set; }
}

public class GetOrderListPostingProduct
{
    [JsonProperty("quantity")] public int Quantity { get; set; }

    [JsonProperty("offer_id")] public string OfferId { get; set; }

    [JsonProperty("price")] public string Price { get; set; }
}

public class GetOrderListPostingCustomer
{
    [JsonProperty("address")] public GetOrderListPostingCustomerAddress Address { get; set; }

    [JsonProperty("name")] public string Name { get; set; }

    [JsonProperty("phone")] public string Phone { get; set; }
}

public class GetOrderListPostingCustomerAddress
{
    [JsonProperty("address_tail")] public string AddressTail { get; set; }
}
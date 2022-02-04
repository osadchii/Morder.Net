using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Clients.Orders.Messages;

public class SearchOrdersData : SberMegaMarketMessageData
{
    public SearchOrdersData(string token) : base(token)
    {
    }

    public SearchOrdersData() : base("")
    {
    }

    [JsonProperty("dateFrom")] public DateTime DateFrom { get; set; }

    [JsonProperty("dateTo")] public DateTime DateTo { get; set; }

    [JsonProperty("count")] public int Count => 5000;

    [JsonProperty("statuses")]
    public IEnumerable<string> Statuses => new[]
    {
        "NEW",
        "CONFIRMED",
        "PACKED",
        "PACKING_EXPIRED",
        "SHIPPED",
        "DELIVERED",
        "MERCHANT_CANCELED",
        "CUSTOMER_CANCELED"
    };
}
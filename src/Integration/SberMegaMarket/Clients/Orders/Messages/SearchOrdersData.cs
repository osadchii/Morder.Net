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
}
using Newtonsoft.Json;

namespace Integration.Kuper.Clients.Stocks.Messages;

public class KuperStockMessage
{
    [JsonProperty("data")]
    public Data[] Data { get; set; }
}

public class Data
{
    [JsonProperty("offer_id")]
    public string OfferId { get; set; }
    [JsonProperty("outlet_id")]
    public string OutletId { get; set; }
    [JsonProperty("stock")]
    public string Stock { get; set; }
}
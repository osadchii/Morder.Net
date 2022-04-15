using Newtonsoft.Json;

namespace Integration.Ozon.Clients.Stocks.Messages;

public class OzonStockRequest
{
    [JsonProperty("stocks")] public IEnumerable<OzonStock> Stocks { get; set; }
}

public class OzonStock
{
    [JsonProperty("offer_id")] public string? OfferId { get; set; }

    [JsonProperty("product_id")] public int? ProductId { get; set; }

    [JsonProperty("stock")] public int Stock { get; set; }
}
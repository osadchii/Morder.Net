using Newtonsoft.Json;

namespace Integration.Ozon.Clients.Stocks.Messages;

public class OzonStockRequest
{
    [JsonProperty("stocks")] public List<OzonStock> Stocks { get; set; }
}

public class OzonStock
{
    [JsonProperty("offer_id")] public string OfferId { get; set; }

    [JsonProperty("product_id")] public int? ProductId { get; set; }

    [JsonProperty("stock")] public int Stock { get; set; }
    
    [JsonProperty("warehouse_id")] public long WarehouseId { get; set; }
}
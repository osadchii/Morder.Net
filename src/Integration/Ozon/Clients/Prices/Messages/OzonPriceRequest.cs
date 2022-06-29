using Newtonsoft.Json;

namespace Integration.Ozon.Clients.Prices.Messages;

public class OzonPriceRequest
{
    [JsonProperty("prices")] public IEnumerable<OzonPrice> Prices { get; set; } = null!;
}

public class OzonPrice
{
    [JsonProperty("offer_id")] public string? OfferId { get; set; }

    [JsonProperty("product_id")] public int? ProductId { get; set; }

    [JsonProperty("old_price")] public string OldPrice { get; set; } = "0";

    [JsonProperty("price")] public string Price { get; set; } = null!;
}
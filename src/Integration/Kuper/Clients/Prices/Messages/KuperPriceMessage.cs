using Newtonsoft.Json;

namespace Integration.Kuper.Clients.Prices.Messages;

public class KuperPriceMessage
{
    [JsonProperty("data")] public Data[] Data { get; set; }
}

public class Data
{
    [JsonProperty("active")] public bool Active { get; set; }
    [JsonProperty("offer_id")] public string OfferId { get; set; }
    [JsonProperty("outlet_id")] public string OutletId { get; set; }
    [JsonProperty("price")] public Price Price { get; set; }
    [JsonProperty("price_category")] public string PriceCategory { get; set; }
    [JsonProperty("price_type")] public string PriceType { get; set; }
    [JsonProperty("vat")] public string Vat { get; set; }
}

public class Price
{
    [JsonProperty("amount")] public string Amount { get; set; }
    [JsonProperty("currency")] public string Currency { get; set; }
}
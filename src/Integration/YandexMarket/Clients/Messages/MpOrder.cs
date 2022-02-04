using Newtonsoft.Json;

namespace Integration.YandexMarket.Clients.Messages;

public class MpOrder
{
    [JsonProperty("number")] public string Number { get; set; } = null!;

    [JsonProperty("fake")] public bool Fake { get; set; }

    [JsonProperty("date")] public DateTime Date { get; set; }

    [JsonProperty("shippingDate")] public DateTime ShippingDate { get; set; }

    [JsonProperty("status")] public string Status { get; set; } = null!;

    [JsonProperty("items")] public IEnumerable<MpOrderItem> Items { get; set; } = null!;
}

public class MpOrderItem
{
    [JsonProperty("offerGUID")] public Guid OfferGuid { get; set; }

    [JsonProperty("count")] public decimal Count { get; set; }

    [JsonProperty("price")] public decimal Price { get; set; }

    [JsonProperty("canceled")] public bool Canceled { get; set; }
}
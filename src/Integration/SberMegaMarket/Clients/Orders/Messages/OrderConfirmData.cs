using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Clients.Orders.Messages;

public class OrderConfirmData : SberMegaMarketMessageData
{
    public OrderConfirmData(string token) : base(token)
    {
    }

    public OrderConfirmData() : base("")
    {
    }

    [JsonProperty("shipments")] public IEnumerable<ConfirmOrderShipment> Shipments { get; set; } = null!;
}

public class ConfirmOrderShipment
{
    [JsonProperty("shipmentId")] public string ShipmentId { get; set; } = null!;

    [JsonProperty("orderCode")] public string OrderCode { get; set; } = null!;

    [JsonProperty("items")] public IEnumerable<ConfirmOrderShipmentItem> Items { get; set; } = null!;
}

public class ConfirmOrderShipmentItem
{
    [JsonProperty("itemIndex")] public string ItemIndex { get; set; } = null!;

    [JsonProperty("offerId")] public string OfferId { get; set; } = null!;
}
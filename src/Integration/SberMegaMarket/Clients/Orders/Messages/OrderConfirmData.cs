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

    [JsonProperty("shipments")] public IEnumerable<ConfirmOrderShipment> Shipments { get; set; }
}

public class ConfirmOrderShipment
{
    [JsonProperty("shipmentId")] public string ShipmentId { get; set; }

    [JsonProperty("orderCode")] public string OrderCode { get; set; }

    [JsonProperty("items")] public IEnumerable<ConfirmOrderShipmentItem> Items { get; set; }
}

public class ConfirmOrderShipmentItem
{
    [JsonProperty("itemIndex")] public string ItemIndex { get; set; }

    [JsonProperty("offerId")] public string OfferId { get; set; }
}
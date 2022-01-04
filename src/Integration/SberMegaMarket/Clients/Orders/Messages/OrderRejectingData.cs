using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Integration.SberMegaMarket.Clients.Orders.Messages;

public class OrderRejectingData : SberMegaMarketMessageData
{
    public OrderRejectingData(string token) : base(token)
    {
    }

    public OrderRejectingData() : base("")
    {
    }

    [JsonProperty("shipments")] public IEnumerable<OrderRejectingShipment> Shipments { get; set; }
}

public class OrderRejectingShipment
{
    [JsonProperty("shipmentId")] public string ShipmentId { get; set; }

    [JsonProperty("items")] public IEnumerable<OrderRejectingShipmentItem> Items { get; set; }

    [JsonProperty("Reason")] public OrderRejectingShipmentReason Reason { get; set; }
}

public class OrderRejectingShipmentItem
{
    [JsonProperty("itemIndex")] public string ItemIndex { get; set; }

    [JsonProperty("offerId")] public string OfferId { get; set; }
}

public class OrderRejectingShipmentReason
{
    [JsonProperty("type")]
    [JsonConverter(typeof(StringEnumConverter))]
    public RejectingReason Type { get; set; }
}

public enum RejectingReason
{
    [EnumMember(Value = "OUT_OF_STOCK")] OutOfStock
}
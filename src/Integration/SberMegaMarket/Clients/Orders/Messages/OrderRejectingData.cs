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

    [JsonProperty("shipments")] public IEnumerable<OrderRejectingShipment> Shipments { get; set; } = null!;
}

public class OrderRejectingShipment
{
    [JsonProperty("shipmentId")] public string ShipmentId { get; set; } = null!;

    [JsonProperty("items")] public IEnumerable<OrderRejectingShipmentItem> Items { get; set; } = null!;

    [JsonProperty("reason")] public OrderRejectingShipmentReason Reason { get; set; } = null!;
}

public class OrderRejectingShipmentItem
{
    [JsonProperty("itemIndex")] public string ItemIndex { get; set; } = null!;

    [JsonProperty("offerId")] public string OfferId { get; set; } = null!;
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
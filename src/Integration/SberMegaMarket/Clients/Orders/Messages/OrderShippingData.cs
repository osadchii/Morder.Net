using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Clients.Orders.Messages;

public class OrderShippingData : SberMegaMarketMessageData
{
    public OrderShippingData(string token) : base(token)
    {
    }

    public OrderShippingData() : base("")
    {
    }

    [JsonProperty("shipments")] public IEnumerable<OrderShippingShipment> Shipments { get; set; } = null!;
}

public class OrderShippingShipment
{
    [JsonProperty("shipmentId")] public string ShipmentId { get; set; } = null!;

    [JsonProperty("boxes")] public IEnumerable<OrderShippingShipmentBox> Boxes { get; set; } = null!;

    [JsonProperty("shipping")] public OrderShippingShipmentShipping Shipping { get; set; } = null!;
}

public class OrderShippingShipmentBox
{
    [JsonProperty("boxIndex")] public int BoxIndex { get; set; }

    [JsonProperty("boxCode")] public string BoxCode { get; set; } = null!;
}

public class OrderShippingShipmentShipping
{
    [JsonProperty("shippingDate")] public string ShippingDate { get; set; } = null!;
}
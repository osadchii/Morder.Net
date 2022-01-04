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

    [JsonProperty("shipments")] public IEnumerable<OrderShippingShipment> Shipments { get; set; }
}

public class OrderShippingShipment
{
    [JsonProperty("shipmentId")] public string ShipmentId { get; set; }

    [JsonProperty("boxes")] public IEnumerable<OrderShippingShipmentBox> Boxes { get; set; }

    [JsonProperty("shipping")] public OrderShippingShipmentShipping Shipping { get; set; }
}

public class OrderShippingShipmentBox
{
    [JsonProperty("boxIndex")] public int BoxIndex { get; set; }

    [JsonProperty("boxCode")] public string BoxCode { get; set; }
}

public class OrderShippingShipmentShipping
{
    [JsonProperty("shippingDate")] public string ShippingDate { get; set; }
}
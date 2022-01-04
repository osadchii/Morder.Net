using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Clients.Orders.Messages;

public class OrderPackingData : SberMegaMarketMessageData
{
    public OrderPackingData(string token) : base(token)
    {
    }

    public OrderPackingData() : base("")
    {
    }

    [JsonProperty("shipments")] public IEnumerable<OrderPackingShipment> Shipments { get; set; }
}

public class OrderPackingShipment
{
    [JsonProperty("shipmentId")] public string ShipmentId { get; set; }

    [JsonProperty("orderCode")] public string OrderCode { get; set; }

    [JsonProperty("items")] public IEnumerable<OrderPackingShipmentItem> Items { get; set; }
}

public class OrderPackingShipmentItem
{
    [JsonProperty("itemIndex")] public string ItemIndex { get; set; }

    [JsonProperty("boxes")] public IEnumerable<OrderPackingShipmentItemBox> Boxes { get; set; }
}

public class OrderPackingShipmentItemBox
{
    [JsonProperty("boxIndex")] public int BoxIndex { get; set; }

    [JsonProperty("boxCode")] public string BoxCode { get; set; }
}
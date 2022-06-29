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

    [JsonProperty("shipments")] public IEnumerable<OrderPackingShipment> Shipments { get; set; } = null!;
}

public class OrderPackingShipment
{
    [JsonProperty("shipmentId")] public string ShipmentId { get; set; } = null!;

    [JsonProperty("orderCode")] public string OrderCode { get; set; } = null!;

    [JsonProperty("items")] public IEnumerable<OrderPackingShipmentItem> Items { get; set; } = null!;
}

public class OrderPackingShipmentItem
{
    [JsonProperty("itemIndex")] public string ItemIndex { get; set; } = null!;

    [JsonProperty("boxes")] public IEnumerable<OrderPackingShipmentItemBox> Boxes { get; set; } = null!;
}

public class OrderPackingShipmentItemBox
{
    [JsonProperty("boxIndex")] public int BoxIndex { get; set; }

    [JsonProperty("boxCode")] public string BoxCode { get; set; } = null!;
}
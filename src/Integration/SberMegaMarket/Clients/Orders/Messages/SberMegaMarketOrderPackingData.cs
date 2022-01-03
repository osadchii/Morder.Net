using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Clients.Orders.Messages;

public class SberMegaMarketOrderPackingData : SberMegaMarketMessageData
{
    public SberMegaMarketOrderPackingData(string token) : base(token)
    {
    }

    public SberMegaMarketOrderPackingData() : base("")
    {
    }

    [JsonProperty("shipments")] public IEnumerable<SberMegaMarketOrderPackingShipment> Shipments { get; set; }
}

public class SberMegaMarketOrderPackingShipment
{
    [JsonProperty("shipmentId")] public string ShipmentId { get; set; }

    [JsonProperty("orderCode")] public string OrderCode { get; set; }

    [JsonProperty("items")] public IEnumerable<SberMegaMarketOrderPackingShipmentItem> Items { get; set; }
}

public class SberMegaMarketOrderPackingShipmentItem
{
    [JsonProperty("itemIndex")] public string ItemIndex { get; set; }

    [JsonProperty("boxes")] public IEnumerable<SberMegaMarketOrderPackingShipmentItemBox> Boxes { get; set; }
}

public class SberMegaMarketOrderPackingShipmentItemBox
{
    [JsonProperty("boxIndex")] public int BoxIndex { get; set; }

    [JsonProperty("boxCode")] public string BoxCode { get; set; }
}
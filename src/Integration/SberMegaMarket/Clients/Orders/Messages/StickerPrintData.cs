using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Clients.Orders.Messages;

public class StickerPrintData : SberMegaMarketMessageData
{
    public StickerPrintData(string token) : base(token)
    {
    }

    public StickerPrintData() : base("")
    {
    }

    [JsonProperty("shipments")] public IEnumerable<StickerPrintShipment> Shipments { get; set; }
}

public class StickerPrintShipment
{
    [JsonProperty("shipmentId")] public string ShipmentId { get; set; }

    [JsonProperty("boxCodes")] public IEnumerable<string> BoxCodes { get; set; }

    [JsonProperty("items")] public IEnumerable<StickerPrintShipmentItem> Items { get; set; }
}

public class StickerPrintShipmentItem
{
    [JsonProperty("itemIndex")] public string ItemIndex { get; set; }

    [JsonProperty("quantity")] public int Quantity { get; set; }

    [JsonProperty("boxes")] public IEnumerable<StickerPrintShipmentItemBox> Boxes { get; set; }
}

public class StickerPrintShipmentItemBox
{
    [JsonProperty("boxIndex")] public string BoxIndex { get; set; }

    [JsonProperty("boxCode")] public string BoxCode { get; set; }
}
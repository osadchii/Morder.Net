using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Orders.Messages;

public class CancelSberMegaMarketOrdersRequest
{
    [JsonProperty("merchantId")] public int MerchantId { get; set; }

    [JsonProperty("shipments")] public IEnumerable<CancelSberMegaMarketOrder> Shipments { get; set; }
}

public class CancelSberMegaMarketOrder
{
    [JsonProperty("shipmentId")] public string ShipmentId { get; set; }

    [JsonProperty("items")] public IEnumerable<CancelSberMegaMarketOrderItem> Items { get; set; }
}

public class CancelSberMegaMarketOrderItem
{
    [JsonProperty("itemIndex")] public string ItemIndex { get; set; }

    [JsonProperty("offerId")] public string OfferId { get; set; }
}
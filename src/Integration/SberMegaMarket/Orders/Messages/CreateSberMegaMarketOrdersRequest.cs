using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Orders.Messages;

public class CreateSberMegaMarketOrdersRequest
{
    [JsonProperty("merchantId")] public int MerchantId { get; set; }

    [JsonProperty("shipments")] public IEnumerable<CreateSberMegaMarketOrder> Shipments { get; set; }
}

public class CreateSberMegaMarketOrder
{
    [JsonProperty("shipmentId")] public string ShipmentId { get; set; }

    [JsonProperty("shipmentDate")] public DateTime ShipmentDate { get; set; }

    [JsonProperty("items")] public IEnumerable<CreateSberMegaMarketOrderItem> Items { get; set; }

    [JsonProperty("shipping")] public CreateSberMegaMarketOrderShipping Shipping { get; set; }
}

public class CreateSberMegaMarketOrderItem
{
    [JsonProperty("itemIndex")] public string ItemIndex { get; set; }

    [JsonProperty("offerId")] public string OfferId { get; set; }

    [JsonProperty("price")] public decimal Price { get; set; }

    [JsonProperty("quantity")] public decimal Quantity { get; set; }
}

public class CreateSberMegaMarketOrderShipping
{
    [JsonProperty("shippingDate")] public DateTime ShippingDate { get; set; }
}
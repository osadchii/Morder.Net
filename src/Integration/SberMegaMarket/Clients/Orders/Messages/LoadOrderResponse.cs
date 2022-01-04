using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Clients.Orders.Messages;

public class LoadOrderResponse
{
    [JsonProperty("success")] public int Success { get; set; }

    [JsonProperty("meta")] public Dictionary<string, object> Meta { get; set; }

    [JsonProperty("data")] public LoadOrderResponseData Data { get; set; }
}

public class LoadOrderResponseData
{
    [JsonProperty("shipments")] public IEnumerable<LoadOrderResponseDataShipment> Shipments { get; set; }
}

public class LoadOrderResponseDataShipment
{
    [JsonProperty("shipmentId")] public string ShipmentId { get; set; }

    [JsonProperty("orderCode")] public string OrderCode { get; set; }

    [JsonProperty("confirmedTimeLimit")] public DateTime ConfirmedTimeLimit { get; set; }

    [JsonProperty("packingTimeLimit")] public DateTime PackingTimeLimit { get; set; }

    [JsonProperty("shippingTimeLimit")] public DateTime ShippingTimeLimit { get; set; }

    [JsonProperty("customerFullName")] public string CustomerFullName { get; set; }

    [JsonProperty("customerAddress")] public string CustomerAddress { get; set; }

    [JsonProperty("items")] public IEnumerable<LoadOrderResponseDataShipmentItem> Items { get; set; }
}

public class LoadOrderResponseDataShipmentItem
{
    [JsonProperty("itemIndex")] public string ItemIndex { get; set; }

    [JsonProperty("status")] public string Status { get; set; }
}
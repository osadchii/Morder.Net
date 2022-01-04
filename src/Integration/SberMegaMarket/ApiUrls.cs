namespace Integration.SberMegaMarket;

public static class ApiUrls
{
    public const string ConfirmOrder = "/api/order/confirm";
    public const string PackingOrder = "/api/order/packing";
    public const string RejectOrder = "/api/order/reject";
    public const string ShippingOrder = "/api/order/shipping";

    public const string SendStocks = "/api/merchantIntegration/v1/offerService/stock/update";
    public const string SendPrices = "/api/merchantIntegration/v1/offerService/manualPrice/save";
}
namespace Integration.SberMegaMarket;

public static class ApiUrls
{
    public const string ConfirmOrder = "/api/market/v1/orderService/order/confirm";
    public const string PackingOrder = "/api/market/v1/orderService/order/packing";
    public const string RejectOrder = "/api/market/v1/orderService/order/reject";
    public const string ShippingOrder = "/api/market/v1/orderService/order/shipping";
    public const string StickerPrint = "/api/market/v1/orderService/sticker/print";

    public const string SendStocks = "/api/merchantIntegration/v1/offerService/stock/update";
    public const string SendPrices = "/api/merchantIntegration/v1/offerService/manualPrice/save";
}
namespace Integration.SberMegaMarket;

public static class ApiUrls
{
    private const string BaseOrder = "/api/market/v1/orderService";

    public const string ConfirmOrder = BaseOrder + "/order/confirm";
    public const string PackingOrder = BaseOrder + "/order/packing";
    public const string RejectOrder = BaseOrder + "/order/reject";
    public const string ShippingOrder = BaseOrder + "/order/shipping";
    public const string StickerPrint = BaseOrder + "/sticker/print";
    public const string GetOrders = BaseOrder + "/order/get";
    public const string SearchOrders = BaseOrder + "/order/search";

    public const string SendStocks = "/api/merchantIntegration/v1/offerService/stock/update";
    public const string SendPrices = "/api/merchantIntegration/v1/offerService/manualPrice/save";
}
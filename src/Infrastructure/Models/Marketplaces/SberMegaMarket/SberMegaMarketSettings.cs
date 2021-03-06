namespace Infrastructure.Models.Marketplaces.SberMegaMarket;

public class SberMegaMarketSettings
{
    public string Token { get; set; } = "";
    public int MerchantId { get; set; } = 0;
    public bool FeedEnabled { get; set; } = false;
    public string FeedName { get; set; } = "";
    public string Server { get; set; } = "";
    public int Port { get; set; } = 443;
    public int WarehouseId { get; set; } = 1;
    public int OrderBefore { get; set; } = 1;
    public int ShippingDays { get; set; } = 1;
    public bool LoadOrdersAsArchived { get; set; }
    public bool LoadOrders { get; set; }
}
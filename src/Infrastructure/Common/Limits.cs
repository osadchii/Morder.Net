namespace Infrastructure.Common;

public static class Limits
{
    public const int CategoryName = 256;

    public const int ProductName = 256;
    public const int ProductArticul = 100;
    public const int ProductBrand = 256;
    public const int ProductBarcode = 200;
    public const int ProductVendor = 256;
    public const int ProductVendorCode = 100;
    public const int ProductCountryOfOrigin = 128;

    public const int MinimalLength = 0;
    public const int MaximalLength = 1_000_000;
    public const int MinimalWidth = 0;
    public const int MaximalWidth = 1_000_000;
    public const int MinimalHeight = 0;
    public const int MaximalHeight = 1_000_000;
    public const int MinimalWeight = 0;
    public const int MaximalWeight = 1_000_000;
    public const int MinimalVolume = 0;
    public const int MaximalVolume = 1_000_000;

    public const int PriceTypeName = 128;
    public const int MinimalPrice = 0;
    public const int MaximalPrice = int.MaxValue;

    public const int WarehouseName = 128;
    public const int MinimalStock = 0;
    public const int MaximalStock = int.MaxValue;

    public const int CompanyName = 256;
    public const int ShopName = 256;
    public const int ShopUrl = 256;

    public const int BotUserUserName = 128;
    public const int BotUserFirstName = 128;
    public const int BotUserLastName = 128;
    public const int BotUserCurrentState = 128;
    public const int BotUserCurrentStateKey = 36;

    public const int OrderNumber = 36;
    public const int OrderCustomer = 256;
    public const int OrderCustomerAddress = 512;
    public const int OrderMinimalPrice = 0;
    public const int OrderMaximalPrice = int.MaxValue;
    public const int OrderMinimalCount = 0;
    public const int OrderMaximalCount = int.MaxValue;
    public const int OrderBoxMinimalNumber = 1;
    public const int OrderBoxMaximalNumber = 1000;
    public const int OrderStickerName = 256;

    public const int MarketplaceName = 128;

    public const int OzonWarehouseName = 128;

    public const int MarketplaceProductSettingExternalId = 128;

    public const int UserNameLength = 128;
    public const int UserPasswordLength = 128;

    public const int ProductIdentifierValueLength = 128;
}
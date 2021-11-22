namespace Infrastructure.Common;

public static class Limits
{
    public const int CategoryName = 256;
    
    public const int ProductName = 256;

    public const int PriceTypeName = 128;
    public const int MinimalPrice = 0;
    public const int MaximalPrice = Int32.MaxValue;

    public const int WarehouseName = 128;
    public const int MinimalStock = 0;
    public const int MaximalStock = Int32.MaxValue;

    public const int CompanyName = 256;
    public const int ShopName = 256;
    public const int ShopUrl = 256;
}
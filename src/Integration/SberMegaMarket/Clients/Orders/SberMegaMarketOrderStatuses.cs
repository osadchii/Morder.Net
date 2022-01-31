namespace Integration.SberMegaMarket.Clients.Orders;

public static class SberMegaMarketOrderStatuses
{
    public const string CustomerCanceled = "CUSTOMER_CANCELED";
    public const string MerchantCanceled = "MERCHANT_CANCELED";
    public const string Delivered = "DELIVERED";
    public const string PendingConfirmation = "PENDING_CONFIRMATION";
    public const string Confirmed = "CONFIRMED";
    public const string PendingPacking = "PENDING_PACKING";
    public const string Packed = "PACKED";
    public const string Shipped = "SHIPPED";

    public static bool IsCanceled(string status)
    {
        return status is CustomerCanceled or MerchantCanceled;
    }
}
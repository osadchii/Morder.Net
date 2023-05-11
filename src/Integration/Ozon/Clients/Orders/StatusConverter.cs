using Infrastructure.Models.Orders;

namespace Integration.Ozon.Clients.Orders;

public static class StatusConverter
{
    public static OrderStatus OzonStatusToOrderStatus(string ozonStatus)
    {
        return ozonStatus switch
        {
            "acceptance_in_progress" => OrderStatus.Shipped,
            "awaiting_approve" => OrderStatus.Created,
            "awaiting_packaging" => OrderStatus.Reserved,
            "awaiting_deliver" => OrderStatus.Packed,
            "awaiting_registration" => OrderStatus.Packed,
            "arbitration" => OrderStatus.Shipped,
            "client_arbitration" => OrderStatus.Shipped,
            "delivering" => OrderStatus.Shipped,
            "driver_pickup" => OrderStatus.Shipped,
            "delivered" => OrderStatus.Finished,
            "cancelled" => OrderStatus.Canceled,
            "not_accepted" => OrderStatus.Shipped,
            _ => OrderStatus.Created
        };
    }
}
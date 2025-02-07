using Infrastructure.Models.Orders;
using static Infrastructure.Models.Orders.OrderStatus;

namespace Integration.Kuper.Clients.Orders;

public static class StatusConverter
{
    public static OrderStatus KuperStatusToOrderStatus(string status)
    {
        return status.ToLowerInvariant().Trim() switch
        {
            "ready" => Created,
            "accepted" => Reserved,
            "canceled" => Canceled,
            "assembled" => Packed,
            "collecting" => Reserved,
            "pending" => Created,
            "ready_to_ship" => Packed,
            "shipped" => Finished,
            "shipping" => Shipped,
            _ => Created
        };
    }
}
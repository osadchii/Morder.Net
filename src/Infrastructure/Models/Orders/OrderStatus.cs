namespace Infrastructure.Models.Orders;

public enum OrderStatus
{
    Created,
    Reserved,
    Packed,
    Shipped,
    Finished,
    Canceled
}
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models.Orders;

public class OrderChange : BaseEntity
{
    [ForeignKey("Order")] public int OrderId { get; set; }
    public Order Order { get; set; }
}
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models.Orders;

[Table("OrderChange", Schema = "dbo")]
public class OrderChange : BaseEntity
{
    [ForeignKey("Order")] public int OrderId { get; set; }
    public Order Order { get; set; }
}
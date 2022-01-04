using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Models.Interfaces;

namespace Infrastructure.Models.Orders;

[Table("OrderStatusHistory", Schema = "dbo")]
public class OrderStatusHistory : BaseEntity, IHasId
{
    [Key] public int Id { get; set; }

    [Required] [ForeignKey("Order")] public int OrderId { get; set; }

    public Order Order { get; set; }

    [Required] public DateTime Date { get; set; }

    [Required] public OrderStatus Status { get; set; }
}
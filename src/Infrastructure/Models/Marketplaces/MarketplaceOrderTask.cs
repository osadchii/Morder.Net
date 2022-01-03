using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Models.Interfaces;
using Infrastructure.Models.Orders;

namespace Infrastructure.Models.Marketplaces;

[Table("MarketplaceOrderTask", Schema = "dbo")]
public class MarketplaceOrderTask : BaseEntity, IHasId
{
    [Key] public int Id { get; set; }

    [Required] public DateTime Date { get; set; }

    [Required] public TaskType Type { get; set; }

    [Required] [ForeignKey("Marketplace")] public int MarketplaceId { get; set; }

    public Marketplace Marketplace { get; set; }

    [Required] [ForeignKey("Order")] public int OrderId { get; set; }

    public Order Order { get; set; }

    public int TryCount { get; set; } = 0;
}

public enum TaskType
{
    Confirm,
    Pack,
    Ship,
    Reject,
    Sticker
}
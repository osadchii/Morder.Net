using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Common;

namespace Infrastructure.Models.Orders;

[Table("OrderSticker", Schema = "dbo")]
public class OrderSticker : BaseEntity
{
    [Required] [ForeignKey("Order")] public int OrderId { get; set; }

    public Order Order { get; set; } = null!;

    [Required]
    [MaxLength(Limits.OrderStickerName)]
    public string Name { get; set; } = null!;

    public byte[] StickerData { get; set; } = null!;
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Common;
using Infrastructure.Models.Interfaces;

namespace Infrastructure.Models.Marketplaces.Ozon;

public class OzonWarehouse : BaseEntity, IHasId
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [ForeignKey("Ozon")]
    public int OzonId { get; set; }
    public Marketplace Ozon { get; set; }

    [Required]
    [MaxLength(Limits.OzonWarehouseName)]
    public string Name { get; set; }
    
    [Required]
    public long OzonWarehouseId { get; set; }
}
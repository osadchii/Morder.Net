using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Models.Interfaces;
using Infrastructure.Models.Products;

namespace Infrastructure.Models.Marketplaces.Ozon;

public class OzonWarehouseBlacklist : BaseEntity, IHasId
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [ForeignKey("OzonWarehouse")]
    public int OzonWarehouseId { get; set; }
    public OzonWarehouse OzonWarehouse { get; set; }
    
    [Required]
    [ForeignKey("Product")]
    public int ProductId { get; set; }
    public Product Product { get; set; }
}
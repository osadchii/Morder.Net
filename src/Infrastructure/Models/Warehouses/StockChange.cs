using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Products;

namespace Infrastructure.Models.Warehouses;

[Table("StockChange", Schema = "dbo")]
public class StockChange
{
    [Required] [ForeignKey("Marketplace")] public int MarketplaceId { get; set; }

    [Required] [ForeignKey("Warehouse")] public int WarehouseId { get; set; }

    [Required] [ForeignKey("Product")] public int ProductId { get; set; }

    public Marketplace Marketplace { get; set; }
    public Warehouse Warehouse { get; set; }
    public Product Product { get; set; }
}
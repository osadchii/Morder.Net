using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Common;
using Infrastructure.Models.Products;

namespace Infrastructure.Models.Warehouses;

[Table("Stock", Schema = "dbo")]
public class Stock : BaseEntity
{
    [ForeignKey("Product")] public int ProductId { get; set; }

    public Product Product { get; set; } = null!;

    [ForeignKey("Warehouse")] public int WarehouseId { get; set; }

    public Warehouse Warehouse { get; set; } = null!;

    [Range(Limits.MinimalStock, Limits.MaximalStock)]
    public decimal Value { get; set; }
}
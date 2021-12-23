using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Common;
using Infrastructure.Marketplaces;
using Infrastructure.Models.Interfaces;
using Infrastructure.Models.Prices;
using Infrastructure.Models.Warehouses;

namespace Infrastructure.Models.Marketplaces;

[Table("Marketplace", Schema = "dbo")]
public class Marketplace : BaseEntity, IHasId
{
    [Key] public int Id { get; set; }

    [Required]
    [MaxLength(Limits.MarketplaceName)]
    public string Name { get; set; }

    [Required] public MarketplaceType Type { get; set; }

    [Required] public string ProductTypes { get; set; }

    [Required] public decimal MinimalPrice { get; set; }
    [Required] public decimal MinimalStock { get; set; }

    [Required] public string Settings { get; set; }

    [Required] [ForeignKey("Warehouse")] public int WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; }

    public bool IsActive { get; set; }
    public bool NullifyStocks { get; set; }
    public bool StockChangesTracking { get; set; }
    public bool PriceChangesTracking { get; set; }
    public int StockSendLimit { get; set; }
    public int PriceSendLimit { get; set; }

    [ForeignKey("PriceType")] public int? PriceTypeId { get; set; }
    public PriceType? PriceType { get; set; }
}
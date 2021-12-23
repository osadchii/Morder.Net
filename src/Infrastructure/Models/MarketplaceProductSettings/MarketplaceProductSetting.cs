using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Common;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Products;

namespace Infrastructure.Models.MarketplaceProductSettings;

[Table("MarketplaceProductSetting", Schema = "dbo")]
public class MarketplaceProductSetting : BaseEntity
{
    [ForeignKey("Product")] public int ProductId { get; set; }

    public Product Product { get; set; }

    [ForeignKey("Marketplace")] public int MarketplaceId { get; set; }
    public Marketplace Marketplace { get; set; }

    public bool NullifyStock { get; set; }
    public bool IgnoreRestrictions { get; set; }

    [MaxLength(Limits.MarketplaceProductSettingExternalId)]
    public string? ExternalId { get; set; }
}
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Products;

namespace Infrastructure.Models.MarketplaceCategorySettings;

[Table("MarketplaceCategorySetting", Schema = "dbo")]
public class MarketplaceCategorySetting : BaseEntity
{
    [ForeignKey("Category")] public int CategoryId { get; set; }

    public Category Category { get; set; } = null!;

    [ForeignKey("Marketplace")] public int MarketplaceId { get; set; }
    public Marketplace Marketplace { get; set; } = null!;

    public bool Blocked { get; set; }
}
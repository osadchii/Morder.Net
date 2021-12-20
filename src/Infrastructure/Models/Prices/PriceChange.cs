using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Products;

namespace Infrastructure.Models.Prices;

[Table("PriceChange", Schema = "dbo")]
public class PriceChange : BaseEntity
{
    [Required] [ForeignKey("Marketplace")] public int MarketplaceId { get; set; }

    [Required] [ForeignKey("PriceType")] public int PriceTypeId { get; set; }

    [Required] [ForeignKey("Product")] public int ProductId { get; set; }

    public Marketplace Marketplace { get; set; }
    public PriceType PriceType { get; set; }
    public Product Product { get; set; }
}
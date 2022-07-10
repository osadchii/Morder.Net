using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Common;
using Infrastructure.Models.Marketplaces;

namespace Infrastructure.Models.Products;

public class ProductIdentifier : BaseEntity
{
    [ForeignKey("Product")]
    public int ProductId { get; set; }

    public Product Product { get; set; } = null!;
    
    [ForeignKey("Marketplace")]
    public int MarketplaceId { get; set; }

    public Marketplace Marketplace { get; set; } = null!;
    
    public ProductIdentifierType Type { get; set; }

    [MaxLength(Limits.ProductIdentifierValueLength)]
    public string Value { get; set; } = null!;
}

public enum ProductIdentifierType
{
    StockAndPrice,
    OzonFbo,
    OzonFbs
}
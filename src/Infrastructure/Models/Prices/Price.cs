using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Common;
using Infrastructure.Models.Products;

namespace Infrastructure.Models.Prices;

[Table("Price", Schema = "dbo")]
public class Price : BaseEntity
{
    [ForeignKey("Product")] public int ProductId { get; set; }

    public Product Product { get; set; } = null!;

    [ForeignKey("PriceType")] public int PriceTypeId { get; set; }

    public PriceType PriceType { get; set; } = null!;

    [Range(Limits.MinimalPrice, Limits.MaximalPrice)]
    public decimal Value { get; set; }
}
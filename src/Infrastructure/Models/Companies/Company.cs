using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Common;
using Infrastructure.Models.Interfaces;
using Infrastructure.Models.Prices;

namespace Infrastructure.Models.Companies;

[Table("Company", Schema = "dbo")]
public class Company : BaseEntity, IHasId
{
    [Key] public int Id { get; set; }

    [Required]
    [MaxLength(Limits.CompanyName)]
    public string Name { get; set; }

    [Required]
    [MaxLength(Limits.ShopName)]
    public string Shop { get; set; }

    [MaxLength(Limits.ShopUrl)] public string Url { get; set; }

    [ForeignKey("PriceType")] public int? PriceTypeId { get; set; }

    public PriceType PriceType { get; set; }
}
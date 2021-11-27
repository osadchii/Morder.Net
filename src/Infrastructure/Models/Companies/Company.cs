using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Common;
using Infrastructure.Interfaces;

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
}
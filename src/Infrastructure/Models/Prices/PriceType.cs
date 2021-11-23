using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Common;
using Infrastructure.Interfaces;

namespace Infrastructure.Models.Prices;

[Table("PriceType", Schema = "dbo")]
public class PriceType : IHasId, IHasExternalId
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(Limits.PriceTypeName)]
    public string Name { get; set; }

    public Guid ExternalId { get; set; }
}
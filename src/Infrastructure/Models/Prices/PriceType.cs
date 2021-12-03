using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Common;
using Infrastructure.Models.Interfaces;

namespace Infrastructure.Models.Prices;

[Table("PriceType", Schema = "dbo")]
public class PriceType : BaseEntity, IHasId, IHasExternalId, IHasDeletionMark
{
    [Key] public int Id { get; set; }

    [Required]
    [MaxLength(Limits.PriceTypeName)]
    public string? Name { get; set; }

    [Required] public Guid ExternalId { get; set; }

    public bool DeletionMark { get; set; }
}
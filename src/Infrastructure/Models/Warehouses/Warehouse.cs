using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Common;
using Infrastructure.Models.Interfaces;

namespace Infrastructure.Models.Warehouses;

[Table("Warehouse", Schema = "dbo")]
public class Warehouse : BaseEntity, IHasId, IHasExternalId, IHasDeletionMark
{
    [Key] public int Id { get; set; }

    [Required]
    [MaxLength(Limits.WarehouseName)]
    public string? Name { get; set; }

    [Required] public Guid ExternalId { get; set; }

    public bool DeletionMark { get; set; }
}
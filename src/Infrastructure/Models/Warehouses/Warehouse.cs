using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Common;
using Infrastructure.Interfaces;

namespace Infrastructure.Models.Warehouses;

[Table("Warehouse", Schema = "dbo")]
public class Warehouse : IHasId, IHasExternalId, IHasDeletionMark
{
    [Key] public int Id { get; set; }

    [Required]
    [MaxLength(Limits.WarehouseName)]
    public string Name { get; set; }

    public Guid ExternalId { get; set; }

    public bool DeletionMark { get; set; }
}
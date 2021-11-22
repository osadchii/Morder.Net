using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Common;
using Infrastructure.Interfaces;

namespace Infrastructure.Models.Warehouses;

[Table("Warehouse", Schema = "dbo")]
public class Warehouse : IHasId
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] 
    [MaxLength(Limits.WarehouseName)]
    public string Name { get; set; }
}
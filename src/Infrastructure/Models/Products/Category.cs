using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Common;
using Infrastructure.Models.Interfaces;

namespace Infrastructure.Models.Products;

[Table("Category", Schema = "dbo")]
public class Category : BaseEntity, IHasId, IHasExternalId, IHasDeletionMark
{
    [Key] public int Id { get; set; }

    [Required]
    [MaxLength(Limits.CategoryName)]
    public string? Name { get; set; }

    [MaxLength(100)] public string? TestField { get; set; }

    [ForeignKey("Parent")] public int? ParentId { get; set; }

    public Category? Parent { get; set; }

    public bool DeletionMark { get; set; }

    [Required] public Guid ExternalId { get; set; }
}
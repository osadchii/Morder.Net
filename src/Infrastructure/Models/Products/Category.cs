using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Common;
using Infrastructure.Interfaces;

namespace Infrastructure.Models.Products;

[Table("Category", Schema = "dbo")]
public class Category : IHasId, IHasExternalId, IHasDeletionMark
{
    [Key] public int Id { get; set; }

    [Required]
    [MaxLength(Limits.CategoryName)]
    public string Name { get; set; }

    [ForeignKey("Parent")] public int? ParentId { get; set; }

    public Category? Parent { get; set; }

    public bool DeletionMark { get; set; }

    public Guid ExternalId { get; set; }
}
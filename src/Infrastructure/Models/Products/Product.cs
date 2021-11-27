using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Common;
using Infrastructure.Interfaces;

namespace Infrastructure.Models.Products;

[Table("Product", Schema = "dbo")]
public class Product : IHasId, IHasExternalId, IHasDeletionMark
{
    [Key] public int Id { get; set; }

    [Required]
    [MaxLength(Limits.ProductName)]
    public string Name { get; set; }

    [Required]
    [MaxLength(Limits.ProductArticul)]
    public string Articul { get; set; }

    [ForeignKey("Category")] public int? CategoryId { get; set; }

    public Category? Category { get; set; }

    public bool DeletionMark { get; set; }

    public Guid ExternalId { get; set; }
}
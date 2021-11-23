using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Common;
using Infrastructure.Interfaces;

namespace Infrastructure.Models.Products;

[Table("Product", Schema = "dbo")]
public class Product : IHasId, IHasExternalId
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

    [Required]
    [MaxLength(Limits.ProductName)]
    public string Name { get; set; }

    [ForeignKey("Category")] public int? CategoryId { get; set; }

    public Category? Category { get; set; }

    public bool IsDeleted { get; set; }

    public Guid ExternalId { get; set; }
}
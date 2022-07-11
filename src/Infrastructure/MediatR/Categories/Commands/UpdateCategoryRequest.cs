using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using Infrastructure.Models.Products;
using MediatR;

namespace Infrastructure.MediatR.Categories.Commands;

public class UpdateCategoryRequest : IRequest
{
    [Required]
    [MaxLength(Limits.CategoryName)]
    public string Name { get; set; }

    public Guid? ParentId { get; set; }

    public bool DeletionMark { get; set; }

    [Required] public Guid? ExternalId { get; set; }

    public Category Parent { get; set; }
}
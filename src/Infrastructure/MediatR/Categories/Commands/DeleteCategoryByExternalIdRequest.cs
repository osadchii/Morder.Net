using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.Products;
using MediatR;

namespace Infrastructure.MediatR.Categories.Commands;

public class DeleteCategoryByExternalIdRequest : IRequest<CategoryDto>
{
    [Required] public Guid? ExternalId { get; set; }
}
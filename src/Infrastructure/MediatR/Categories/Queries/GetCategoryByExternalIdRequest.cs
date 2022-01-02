using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.Products;
using MediatR;

namespace Infrastructure.MediatR.Categories.Queries;

public class GetCategoryByExternalIdRequest : IRequest<CategoryDto>
{
    [Required] public Guid? ExternalId { get; set; }
}
using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.Products;
using MediatR;

namespace Infrastructure.MediatR.Categories.Queries;

public class GetCategoryByIdRequest : IRequest<CategoryDto>
{
    [Required] public int? Id { get; set; }
}
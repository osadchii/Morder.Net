using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.Products;
using MediatR;

namespace Infrastructure.MediatR.Categories.Commands;

public class DeleteCategoryByIdRequest : IRequest<CategoryDto>
{
    [Required] public int? Id { get; set; }
}
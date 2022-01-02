using Infrastructure.Models.Products;
using MediatR;

namespace Infrastructure.MediatR.Categories.Queries;

public class GetChildrenCategoriesByParentIdRequest : IRequest<IEnumerable<CategoryDto>>
{
    public int? ParentId { get; set; }
}
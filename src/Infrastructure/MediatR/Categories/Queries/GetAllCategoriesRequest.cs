using Infrastructure.Models.Products;
using MediatR;

namespace Infrastructure.MediatR.Categories.Queries;

public class GetAllCategoriesRequest : IRequest<List<CategoryDto>>
{
}
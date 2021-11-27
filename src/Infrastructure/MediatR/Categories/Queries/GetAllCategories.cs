using Infrastructure.Models.Products;
using MediatR;

namespace Infrastructure.MediatR.Categories.Queries;

public class GetAllCategories : IRequest<List<CategoryDto>>
{
}
using Infrastructure.Models.Products;
using MediatR;

namespace Infrastructure.MediatR.Products.Queries;

public class GetProductByExternalIdRequest : IRequest<ProductDto>
{
    public Guid ExternalId { get; set; }
}
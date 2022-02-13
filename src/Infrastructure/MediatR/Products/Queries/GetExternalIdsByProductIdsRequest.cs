using MediatR;

namespace Infrastructure.MediatR.Products.Queries;

public class GetExternalIdsByProductIdsRequest : IRequest<Dictionary<int, string>>
{
    public int MarketplaceId { get; set; }
    public IEnumerable<int> ProductIds { get; set; } = null!;
}
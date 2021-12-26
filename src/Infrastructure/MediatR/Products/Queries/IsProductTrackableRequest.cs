using MediatR;

namespace Infrastructure.MediatR.Products.Queries;

public class IsProductTrackableRequest : IRequest<bool>
{
    public int MarketplaceId { get; set; }
    public int ProductId { get; set; }
}
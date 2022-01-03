using MediatR;

namespace Infrastructure.MediatR.Orders.Marketplace.Commands;

public class CancelOrderItemsByExternalIdRequest : IRequest<Unit>
{
    public int MarketplaceId { get; set; }
    public string OrderNumber { get; set; }
    public IEnumerable<string> ItemExternalIds { get; set; }
}
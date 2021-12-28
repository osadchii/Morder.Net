using MediatR;

namespace Infrastructure.MediatR.MarketplaceProductSettings.Commands;

public class SetMarketplaceProductExternalIdsRequest : IRequest<Unit>
{
    // Key - articul, value - external id
    public Dictionary<string, string> ExternalIds { get; set; }

    public int MarketplaceId { get; set; }
}
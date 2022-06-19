using MediatR;

namespace Infrastructure.MediatR.MarketplaceProductSettings.Commands;

public class SetMarketplaceProductExternalIdsRequest : IRequest<Unit>
{
    // Key - articul, value - external id
    public Dictionary<string, string> ExternalIds { get; set; } = null!;

    public int MarketplaceId { get; set; }
}
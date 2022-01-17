using MediatR;

namespace Infrastructure.Bot.MediatR.Marketplaces.Queries;

public class GetMarketplaceInformationRequest : IRequest<string>
{
    public int MarketplaceId { get; set; }
}
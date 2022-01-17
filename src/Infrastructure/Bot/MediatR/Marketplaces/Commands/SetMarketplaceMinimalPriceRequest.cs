using MediatR;

namespace Infrastructure.Bot.MediatR.Marketplaces.Commands;

public class SetMarketplaceMinimalPriceRequest : IRequest<Unit>
{
    public int MarketplaceId { get; set; }
    public decimal MinimalPrice { get; set; }
    public long UserChatId { get; set; }
}
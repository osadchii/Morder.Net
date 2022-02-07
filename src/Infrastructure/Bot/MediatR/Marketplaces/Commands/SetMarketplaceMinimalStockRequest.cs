using MediatR;

namespace Infrastructure.Bot.MediatR.Marketplaces.Commands;

public class SetMarketplaceMinimalStockRequest : IRequest<Unit>
{
    public int MarketplaceId { get; set; }
    public decimal MinimalStock { get; set; }
    public long ChatId { get; set; }
}
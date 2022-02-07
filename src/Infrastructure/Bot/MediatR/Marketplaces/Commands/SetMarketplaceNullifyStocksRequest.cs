using MediatR;

namespace Infrastructure.Bot.MediatR.Marketplaces.Commands;

public class SetMarketplaceNullifyStocksRequest : IRequest<Unit>
{
    public int MarketplaceId { get; set; }
    public bool NullifyStocks { get; set; }
    public long ChatId { get; set; }
}
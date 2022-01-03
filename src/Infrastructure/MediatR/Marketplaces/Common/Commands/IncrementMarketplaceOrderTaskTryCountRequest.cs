using MediatR;

namespace Infrastructure.MediatR.Marketplaces.Common.Commands;

public class IncrementMarketplaceOrderTaskTryCountRequest : IRequest<Unit>
{
    public int MarketplaceOrderTaskId { get; set; }
}
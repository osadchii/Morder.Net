using MediatR;

namespace Infrastructure.MediatR.Marketplaces.Common.Commands;

public class DeleteMarketplaceOrderTaskRequest : IRequest<Unit>
{
    public int MarketplaceOrderTaskId { get; set; }
}
using MediatR;

namespace Infrastructure.MediatR.Orders.Marketplace.Common.Queries;

public class OrdersDoNotExistRequest : IRequest<IEnumerable<string>>
{
    public int MarketplaceId { get; set; }
    public IEnumerable<string> Numbers { get; set; } = null!;
}
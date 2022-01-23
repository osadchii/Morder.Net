using Infrastructure.MediatR.Orders.Marketplace.Common.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Orders.Marketplace.Common.Handlers;

public class OrdersDoesNotExistsFilterHandler : IRequestHandler<OrdersDoesNotExistsRequest, IEnumerable<string>>
{
    private readonly MContext _context;

    public OrdersDoesNotExistsFilterHandler(MContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<string>> Handle(OrdersDoesNotExistsRequest request,
        CancellationToken cancellationToken)
    {
        List<string> exists = await _context.Orders
            .AsNoTracking()
            .Where(o => o.MarketplaceId == request.MarketplaceId && request.Numbers.Contains(o.Number))
            .Select(o => o.Number)
            .ToListAsync(cancellationToken);

        return request.Numbers.Except(exists);
    }
}
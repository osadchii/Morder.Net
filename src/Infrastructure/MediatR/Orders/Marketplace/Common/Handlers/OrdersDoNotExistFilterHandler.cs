using Infrastructure.MediatR.Orders.Marketplace.Common.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Orders.Marketplace.Common.Handlers;

public class OrdersDoNotExistFilterHandler : IRequestHandler<OrdersDoNotExistRequest, IEnumerable<string>>
{
    private readonly MContext _context;

    public OrdersDoNotExistFilterHandler(MContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<string>> Handle(OrdersDoNotExistRequest request,
        CancellationToken cancellationToken)
    {
        var exists = await _context.Orders
            .AsNoTracking()
            .Where(o => o.MarketplaceId == request.MarketplaceId && request.Numbers.Contains(o.Number))
            .Select(o => o.Number)
            .ToListAsync(cancellationToken);

        return request.Numbers.Except(exists);
    }
}
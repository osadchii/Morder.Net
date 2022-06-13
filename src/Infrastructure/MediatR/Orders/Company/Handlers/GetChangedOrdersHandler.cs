using Infrastructure.Common;
using Infrastructure.MediatR.Orders.Company.Commands;
using Infrastructure.Models.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Orders.Company.Handlers;

public class GetChangedOrdersHandler : IRequestHandler<GetChangedOrdersRequest, Result>
{
    private readonly MContext _context;

    public GetChangedOrdersHandler(MContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(GetChangedOrdersRequest request, CancellationToken cancellationToken)
    {
        IQueryable<OrderChange> query = request.ClearRegistration
            ? _context.OrderChanges
            : _context.OrderChanges.AsNoTracking();

        List<OrderChange> orders = await query
            .Include(o => o.Order)
            .ThenInclude(o => o.Items)
            .ThenInclude(o => o.Product)
            .Include(o => o.Order)
            .ThenInclude(o => o.Boxes)
            .ThenInclude(b => b.Product)
            .ToListAsync(cancellationToken);

        Result result = orders.Select(o => o.Order).AsResult();

        if (request.ClearRegistration)
        {
            _context.OrderChanges.RemoveRange(orders);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return result;
    }
}
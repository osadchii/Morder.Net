using Infrastructure.MediatR.ChangeTracking.Orders.Commands;
using Infrastructure.Models.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.ChangeTracking.Orders.Handlers;

public class TrackOrdersChangeHandler : IRequestHandler<TrackOrdersChangeRequest, Unit>
{
    private readonly MContext _context;

    public TrackOrdersChangeHandler(MContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(TrackOrdersChangeRequest request, CancellationToken cancellationToken)
    {
        IEnumerable<int> tracked = await _context.OrderChanges
            .AsNoTracking()
            .Where(ot => request.OrderIds.Contains(ot.OrderId))
            .Select(ot => ot.OrderId)
            .ToListAsync(cancellationToken);

        await _context.OrderChanges.AddRangeAsync(request.OrderIds.Except(tracked).Select(id => new OrderChange()
        {
            OrderId = id
        }), cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
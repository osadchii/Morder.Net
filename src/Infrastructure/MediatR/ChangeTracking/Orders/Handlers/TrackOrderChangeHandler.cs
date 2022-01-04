using Infrastructure.MediatR.ChangeTracking.Orders.Commands;
using Infrastructure.Models.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.ChangeTracking.Orders.Handlers;

public class TrackOrderChangeHandler : IRequestHandler<TrackOrderChangeRequest, Unit>
{
    private readonly MContext _context;

    public TrackOrderChangeHandler(MContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(TrackOrderChangeRequest request, CancellationToken cancellationToken)
    {
        bool tracked = await _context.OrderChanges
            .AsNoTracking()
            .AnyAsync(o => o.OrderId == request.OrderId, cancellationToken);

        if (tracked)
        {
            return Unit.Value;
        }

        await _context.OrderChanges.AddAsync(new OrderChange()
        {
            OrderId = request.OrderId
        }, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
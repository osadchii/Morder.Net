using Infrastructure.MediatR.ChangeTracking.Orders.Commands;
using Infrastructure.Models.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.ChangeTracking.Orders.Handlers;

public class TrackOrderChangeHandler : IRequestHandler<TrackOrderChangeRequest, Unit>
{
    private readonly MContext _context;
    private readonly ILogger<TrackOrderChangeHandler> _logger;

    public TrackOrderChangeHandler(MContext context, ILogger<TrackOrderChangeHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(TrackOrderChangeRequest request, CancellationToken cancellationToken)
    {
        var tracked = await _context.OrderChanges
            .AsNoTracking()
            .AnyAsync(o => o.OrderId == request.OrderId, cancellationToken);

        if (tracked)
        {
            _logger.LogInformation("Order {OrderId} already tracked.", request.OrderId);
            return Unit.Value;
        }

        await _context.OrderChanges.AddAsync(new OrderChange()
        {
            OrderId = request.OrderId
        }, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Order {OrderId} tracked.", request.OrderId);

        return Unit.Value;
    }
}
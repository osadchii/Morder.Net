using Infrastructure.MediatR.ChangeTracking.Commands;
using Infrastructure.Models.Prices;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.ChangeTracking.Handlers;

public class TrackPricesChangeHandler : IRequestHandler<TrackPricesChangeRequest, Unit>
{
    private readonly MContext _context;

    public TrackPricesChangeHandler(MContext context)
    {
        _context = context;
    }

    // Don't checking registration conditions
    public async Task<Unit> Handle(TrackPricesChangeRequest request, CancellationToken cancellationToken)
    {
        List<int> alreadyTracked = await _context.PriceChanges
            .AsNoTracking()
            .Where(pc => pc.MarketplaceId == request.MarketplaceId && request.ProductIds.Contains(pc.ProductId))
            .Select(pc => pc.ProductId)
            .ToListAsync(cancellationToken);

        IEnumerable<int> toTrack = request.ProductIds.Except(alreadyTracked);

        await _context.AddRangeAsync(toTrack.Select(h => new PriceChange
        {
            MarketplaceId = request.MarketplaceId,
            ProductId = h
        }), cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
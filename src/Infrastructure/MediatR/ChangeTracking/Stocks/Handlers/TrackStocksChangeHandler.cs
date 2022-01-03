using Infrastructure.MediatR.ChangeTracking.Stocks.Commands;
using Infrastructure.Models.Warehouses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.ChangeTracking.Stocks.Handlers;

public class TrackStocksChangeHandler : IRequestHandler<TrackStocksChangeRequest, Unit>
{
    private readonly MContext _context;

    public TrackStocksChangeHandler(MContext context)
    {
        _context = context;
    }

    // Don't checking registration conditions
    public async Task<Unit> Handle(TrackStocksChangeRequest request, CancellationToken cancellationToken)
    {
        List<int> alreadyTracked = await _context.StockChanges
            .AsNoTracking()
            .Where(pc => pc.MarketplaceId == request.MarketplaceId && request.ProductIds.Contains(pc.ProductId))
            .Select(pc => pc.ProductId)
            .ToListAsync(cancellationToken);

        IEnumerable<int> toTrack = request.ProductIds.Except(alreadyTracked);

        await _context.AddRangeAsync(toTrack.Select(h => new StockChange
        {
            MarketplaceId = request.MarketplaceId,
            ProductId = h
        }), cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
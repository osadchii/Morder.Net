using Infrastructure.MediatR.ChangeTracking.Stocks.Commands;
using Infrastructure.Models.Warehouses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.ChangeTracking.Stocks.Handlers;

public class TrackStocksChangeHandler : IRequestHandler<TrackStocksChangeRequest, Unit>
{
    private readonly MContext _context;
    private readonly ILogger<TrackStocksChangeHandler> _logger;

    public TrackStocksChangeHandler(MContext context, ILogger<TrackStocksChangeHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Don't checking registration conditions
    public async Task<Unit> Handle(TrackStocksChangeRequest request, CancellationToken cancellationToken)
    {
        var alreadyTracked = await _context.StockChanges
            .AsNoTracking()
            .Where(pc => pc.MarketplaceId == request.MarketplaceId && request.ProductIds.Contains(pc.ProductId))
            .Select(pc => pc.ProductId)
            .ToListAsync(cancellationToken);

        var toTrack = request.ProductIds.Except(alreadyTracked).ToArray();

        await _context.AddRangeAsync(toTrack.Select(h => new StockChange
        {
            MarketplaceId = request.MarketplaceId,
            ProductId = h
        }), cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Tracked {Count} stocks to marketplace {Id}", toTrack.Length, request.MarketplaceId);

        return Unit.Value;
    }
}
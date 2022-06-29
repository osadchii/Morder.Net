using Infrastructure.MediatR.ChangeTracking.Prices.Commands;
using Infrastructure.Models.Prices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.ChangeTracking.Prices.Handlers;

public class TrackPricesChangeHandler : IRequestHandler<TrackPricesChangeRequest, Unit>
{
    private readonly MContext _context;
    private readonly ILogger<TrackPricesChangeHandler> _logger;

    public TrackPricesChangeHandler(MContext context, ILogger<TrackPricesChangeHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Don't checking registration conditions
    public async Task<Unit> Handle(TrackPricesChangeRequest request, CancellationToken cancellationToken)
    {
        List<int> alreadyTracked = await _context.PriceChanges
            .AsNoTracking()
            .Where(pc => pc.MarketplaceId == request.MarketplaceId && request.ProductIds.Contains(pc.ProductId))
            .Select(pc => pc.ProductId)
            .ToListAsync(cancellationToken);

        var toTrack = request.ProductIds.Except(alreadyTracked).ToArray();

        await _context.AddRangeAsync(toTrack.Select(h => new PriceChange
        {
            MarketplaceId = request.MarketplaceId,
            ProductId = h
        }), cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Tracked {Count} prices to marketplace {Id}", toTrack.Length, request.MarketplaceId);

        return Unit.Value;
    }
}
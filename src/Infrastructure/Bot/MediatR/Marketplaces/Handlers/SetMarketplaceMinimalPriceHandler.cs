using Infrastructure.Bot.MediatR.Marketplaces.Commands;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Services.Marketplaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Bot.MediatR.Marketplaces.Handlers;

public class SetMarketplaceMinimalPriceHandler : IRequestHandler<SetMarketplaceMinimalPriceRequest, Unit>
{
    private readonly MContext _context;
    private readonly ILogger<SetMarketplaceMinimalPriceHandler> _logger;
    private readonly IChangeTrackingService _changeTrackingService;

    public SetMarketplaceMinimalPriceHandler(MContext context, ILogger<SetMarketplaceMinimalPriceHandler> logger,
        IChangeTrackingService changeTrackingService)
    {
        _context = context;
        _logger = logger;
        _changeTrackingService = changeTrackingService;
    }

    public async Task<Unit> Handle(SetMarketplaceMinimalPriceRequest request, CancellationToken cancellationToken)
    {
        Marketplace marketplace = await _context.Marketplaces
            .SingleAsync(m => m.Id == request.MarketplaceId, cancellationToken);

        decimal oldMinimalPrice = marketplace.MinimalPrice;

        marketplace.MinimalPrice = request.MinimalPrice;

        await _context.SaveChangesAsync(cancellationToken);

        if (marketplace.IsActive && marketplace.StockChangesTracking)
        {
            await _changeTrackingService.TrackStockChangeByMinMaxPrice(marketplace.Id,
                Math.Min(oldMinimalPrice, marketplace.MinimalPrice),
                Math.Max(oldMinimalPrice, marketplace.MinimalPrice), cancellationToken);
        }

        _logger.LogInformation(
            $"User: {request.UserChatId} changed minimal price for ${marketplace.Name} ({marketplace.Id}) to {request.MinimalPrice}");

        return Unit.Value;
    }
}
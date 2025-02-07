using Infrastructure.Bot.MediatR.Commands.Marketplaces;
using Infrastructure.Bot.MediatR.Marketplaces.Commands;
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
    private readonly IMediator _mediator;

    public SetMarketplaceMinimalPriceHandler(MContext context, ILogger<SetMarketplaceMinimalPriceHandler> logger,
        IChangeTrackingService changeTrackingService, IMediator mediator)
    {
        _context = context;
        _logger = logger;
        _changeTrackingService = changeTrackingService;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(SetMarketplaceMinimalPriceRequest request, CancellationToken cancellationToken)
    {
        var marketplace = await _context.Marketplaces
            .SingleAsync(m => m.Id == request.MarketplaceId, cancellationToken);

        var oldMinimalPrice = marketplace.MinimalPrice;

        marketplace.MinimalPrice = request.MinimalPrice;

        await _context.SaveChangesAsync(cancellationToken);

        if (marketplace.IsActive && marketplace.StockChangesTracking)
        {
            await _changeTrackingService.TrackStockChangeByMinMaxPrice(marketplace.Id,
                Math.Min(oldMinimalPrice, marketplace.MinimalPrice),
                Math.Max(oldMinimalPrice, marketplace.MinimalPrice), cancellationToken);
        }

        _logger.LogInformation(
            "User: {ChatId} changed minimal price for {MarketplaceName} ({MarketplaceId}) to {MinimalPrice}",
            request.ChatId, marketplace.Name, marketplace.Id, request.MinimalPrice);

        await _mediator.Send(new ToMarketplaceManagementCommand
        {
            ChatId = request.ChatId,
            MarketplaceId = request.MarketplaceId
        }, cancellationToken);

        return Unit.Value;
    }
}
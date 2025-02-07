using Infrastructure.Bot.MediatR.Commands.Marketplaces;
using Infrastructure.Bot.MediatR.Marketplaces.Commands;
using Infrastructure.Services.Marketplaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Bot.MediatR.Marketplaces.Handlers;

public class SetMarketplaceMinimalStockHandler : IRequestHandler<SetMarketplaceMinimalStockRequest, Unit>
{
    private readonly MContext _context;
    private readonly ILogger<SetMarketplaceMinimalStockHandler> _logger;
    private readonly IChangeTrackingService _changeTrackingService;
    private readonly IMediator _mediator;

    public SetMarketplaceMinimalStockHandler(MContext context, ILogger<SetMarketplaceMinimalStockHandler> logger,
        IChangeTrackingService changeTrackingService, IMediator mediator)
    {
        _context = context;
        _logger = logger;
        _changeTrackingService = changeTrackingService;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(SetMarketplaceMinimalStockRequest request, CancellationToken cancellationToken)
    {
        var marketplace = await _context.Marketplaces
            .SingleAsync(m => m.Id == request.MarketplaceId, cancellationToken);

        var oldMinimalStock = marketplace.MinimalStock;

        marketplace.MinimalStock = request.MinimalStock;

        await _context.SaveChangesAsync(cancellationToken);

        if (marketplace.IsActive && marketplace.StockChangesTracking && oldMinimalStock != marketplace.MinimalStock)
        {
            await _changeTrackingService.TrackAllStocks(marketplace.Id, cancellationToken);
        }

        _logger.LogInformation(
            "User: {ChatId} changed minimal stock for {MarketplaceName} ({MarketplaceId}) to {MinimalStock}",
            request.ChatId, marketplace.Name, marketplace.Id, request.MinimalStock);

        await _mediator.Send(new ToMarketplaceManagementCommand
        {
            ChatId = request.ChatId,
            MarketplaceId = request.MarketplaceId
        }, cancellationToken);

        return Unit.Value;
    }
}
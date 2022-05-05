using Infrastructure.Bot.MediatR.Commands.Marketplaces;
using Infrastructure.Bot.MediatR.Marketplaces.Commands;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Services.Marketplaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Bot.MediatR.Marketplaces.Handlers;

public class SetMarketplaceNullifyStocksHandler : IRequestHandler<SetMarketplaceNullifyStocksRequest, Unit>
{
    private readonly MContext _context;
    private readonly ILogger<SetMarketplaceNullifyStocksHandler> _logger;
    private readonly IChangeTrackingService _changeTrackingService;
    private readonly IMediator _mediator;

    public SetMarketplaceNullifyStocksHandler(MContext context, ILogger<SetMarketplaceNullifyStocksHandler> logger,
        IChangeTrackingService changeTrackingService, IMediator mediator)
    {
        _context = context;
        _logger = logger;
        _changeTrackingService = changeTrackingService;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(SetMarketplaceNullifyStocksRequest request, CancellationToken cancellationToken)
    {
        Marketplace marketplace = await _context.Marketplaces
            .SingleAsync(m => m.Id == request.MarketplaceId, cancellationToken);

        marketplace.NullifyStocks = request.NullifyStocks;

        await _context.SaveChangesAsync(cancellationToken);

        if (marketplace.IsActive && marketplace.StockChangesTracking)
        {
            await _changeTrackingService.TrackAllStocks(request.MarketplaceId, cancellationToken);
        }

        _logger.LogInformation(
            "User: {ChatId} changed nullify stock value for {MarketplaceName} ({MarketplaceId}) to {NullifyStock}",
            request.ChatId, marketplace.Name, marketplace.Id, request.NullifyStocks);

        await _mediator.Send(new ToMarketplaceManagementCommand()
        {
            ChatId = request.ChatId,
            MarketplaceId = request.MarketplaceId
        }, cancellationToken);

        return Unit.Value;
    }
}
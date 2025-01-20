using Infrastructure.MediatR.ChangeTracking.Prices.Commands;
using Infrastructure.MediatR.Products.Queries;
using Infrastructure.Services.Marketplaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.ChangeTracking.Prices.Handlers;

public class TrackAllPricesHandler : IRequestHandler<TrackAllPricesRequest, Unit>
{
    private readonly IMediator _mediator;
    private readonly IChangeTrackingService _trackingService;
    private readonly ILogger<TrackAllPricesHandler> _logger;

    public TrackAllPricesHandler(IMediator mediator, IChangeTrackingService trackingService, ILogger<TrackAllPricesHandler> logger)
    {
        _mediator = mediator;
        _trackingService = trackingService;
        _logger = logger;
    }

    public async Task<Unit> Handle(TrackAllPricesRequest request, CancellationToken cancellationToken)
    {
        var products =
            await _mediator.Send(new GetAllMarketplaceProductIdsRequest { MarketplaceId = request.MarketplaceId },
                cancellationToken);

        await _trackingService.TrackPricesChange(request.MarketplaceId, products, cancellationToken);
        
        _logger.LogInformation("Tracked {Count} prices to marketplace {Id}", products.Count, request.MarketplaceId);

        return Unit.Value;
    }
}
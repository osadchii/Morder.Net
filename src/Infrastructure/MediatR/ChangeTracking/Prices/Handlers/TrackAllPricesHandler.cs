using Infrastructure.MediatR.ChangeTracking.Prices.Commands;
using Infrastructure.MediatR.Products.Queries;
using Infrastructure.Services.Marketplaces;
using MediatR;

namespace Infrastructure.MediatR.ChangeTracking.Prices.Handlers;

public class TrackAllPricesHandler : IRequestHandler<TrackAllPricesRequest, Unit>
{
    private readonly IMediator _mediator;
    private readonly IChangeTrackingService _trackingService;

    public TrackAllPricesHandler(IMediator mediator, IChangeTrackingService trackingService)
    {
        _mediator = mediator;
        _trackingService = trackingService;
    }

    public async Task<Unit> Handle(TrackAllPricesRequest request, CancellationToken cancellationToken)
    {
        List<int> products =
            await _mediator.Send(new GetAllMarketplaceProductIdsRequest { MarketplaceId = request.MarketplaceId },
                cancellationToken);

        await _trackingService.TrackPricesChange(request.MarketplaceId, products, cancellationToken);

        return Unit.Value;
    }
}
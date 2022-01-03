using Infrastructure.MediatR.ChangeTracking.Stocks.Commands;
using Infrastructure.MediatR.Products.Queries;
using Infrastructure.Services.Marketplaces;
using MediatR;

namespace Infrastructure.MediatR.ChangeTracking.Stocks.Handlers;

public class TrackAllStocksHandler : IRequestHandler<TrackAllStocksRequest, Unit>
{
    private readonly IMediator _mediator;
    private readonly IChangeTrackingService _trackingService;

    public TrackAllStocksHandler(IMediator mediator, IChangeTrackingService trackingService)
    {
        _mediator = mediator;
        _trackingService = trackingService;
    }

    public async Task<Unit> Handle(TrackAllStocksRequest request, CancellationToken cancellationToken)
    {
        List<int> products =
            await _mediator.Send(new GetAllMarketplaceProductIdsRequest { MarketplaceId = request.MarketplaceId },
                cancellationToken);

        await _trackingService.TrackStocksChange(request.MarketplaceId, products, cancellationToken);

        return Unit.Value;
    }
}
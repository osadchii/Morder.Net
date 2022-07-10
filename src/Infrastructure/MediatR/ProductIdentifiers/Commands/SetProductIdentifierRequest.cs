using Infrastructure.Models.Products;
using Infrastructure.Services.Marketplaces;
using MediatR;

namespace Infrastructure.MediatR.ProductIdentifiers.Commands;

public class SetProductIdentifierRequest : IRequest<Unit>
{
    public int MarketplaceId { get; set; }
    public int ProductId { get; set; }
    public ProductIdentifierType Type { get; set; }
    public string Value { get; set; } = null!;
}

public class SetProductIdentifierHandler : IRequestHandler<SetProductIdentifierRequest, Unit>
{
    private readonly IProductIdentifierService _identifierService;
    private readonly IChangeTrackingService _trackingService;

    public SetProductIdentifierHandler(IProductIdentifierService identifierService, IChangeTrackingService trackingService)
    {
        _identifierService = identifierService;
        _trackingService = trackingService;
    }

    public async Task<Unit> Handle(SetProductIdentifierRequest request, CancellationToken cancellationToken)
    {
        var result =
            await _identifierService.SetIdentifierAsync(request.MarketplaceId, request.ProductId, request.Type,
                request.Value);

        if (request.Type == ProductIdentifierType.StockAndPrice && result)
        {
            await _trackingService.TrackPriceChange(request.MarketplaceId, request.ProductId, cancellationToken);
            await _trackingService.TrackStockChange(request.MarketplaceId, request.ProductId, cancellationToken);
        }
        
        return Unit.Value;
    }
}
using AutoMapper;
using Infrastructure.Models.Prices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.ChangeTracking.Prices.Commands;

public class TrackPriceChangeRequest : IRequest<Unit>
{
    public int MarketplaceId { get; set; }
    public int ProductId { get; set; }

    public TrackPriceChangeRequest()
    {
    }

    public TrackPriceChangeRequest(int marketplaceId, int productId)
    {
        MarketplaceId = marketplaceId;
        ProductId = productId;
    }
}

public class TrackPriceChangeHandler : IRequestHandler<TrackPriceChangeRequest, Unit>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<TrackPriceChangeHandler> _logger;

    public TrackPriceChangeHandler(MContext context, IMapper mapper, ILogger<TrackPriceChangeHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Unit> Handle(TrackPriceChangeRequest request, CancellationToken cancellationToken)
    {
        var tracked = await _context.PriceChanges
            .AsNoTracking()
            .AnyAsync(t =>
                t.MarketplaceId == request.MarketplaceId
                && t.ProductId == request.ProductId, cancellationToken);

        if (tracked)
        {
            _logger.LogInformation("Price for product with {ProductId} id already tracked for marketplace with {MarketplaceId} id. Skip tracking", request.ProductId, request.MarketplaceId);
            return Unit.Value;
        }

        var change = _mapper.Map<PriceChange>(request);
        await _context.PriceChanges.AddAsync(change, cancellationToken);
        _logger.LogInformation("Price for product with {ProductId} has been tracked for marketplace with {MarketplaceId} id", request.ProductId, request.MarketplaceId);

        return Unit.Value;
    }
}
using AutoMapper;
using Infrastructure.Models.Warehouses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.ChangeTracking.Stocks.Commands;

public class TrackStockChangeRequest : IRequest<Unit>
{
    public int MarketplaceId { get; set; }
    public int ProductId { get; set; }

    public TrackStockChangeRequest()
    {
    }

    public TrackStockChangeRequest(int marketplaceId, int productId)
    {
        MarketplaceId = marketplaceId;
        ProductId = productId;
    }
}

public class TrackStockChangeHandler : IRequestHandler<TrackStockChangeRequest, Unit>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<TrackStockChangeHandler> _logger;

    public TrackStockChangeHandler(MContext context, IMapper mapper, ILogger<TrackStockChangeHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Unit> Handle(TrackStockChangeRequest request, CancellationToken cancellationToken)
    {
        var tracked = await _context.StockChanges
            .AsNoTracking()
            .AnyAsync(t =>
                t.MarketplaceId == request.MarketplaceId
                && t.ProductId == request.ProductId, cancellationToken);

        if (tracked)
        {
            _logger.LogInformation("Stock for product with {ProductId} id already tracked for marketplace with {MarketplaceId} id. Skip tracking", request.ProductId, request.MarketplaceId);
            return Unit.Value;
        }

        var change = _mapper.Map<StockChange>(request);
        await _context.StockChanges.AddAsync(change, cancellationToken);
        _logger.LogInformation("Stock for product with {ProductId} has been tracked for marketplace with {MarketplaceId} id", request.ProductId, request.MarketplaceId);

        return Unit.Value;
    }
}
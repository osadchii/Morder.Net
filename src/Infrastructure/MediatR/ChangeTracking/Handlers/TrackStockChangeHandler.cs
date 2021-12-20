using AutoMapper;
using Infrastructure.MediatR.ChangeTracking.Commands;
using Infrastructure.Models.Warehouses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.ChangeTracking.Handlers;

public class TrackStockChangeHandler : IRequestHandler<TrackStockChangeRequest, Unit>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;

    public TrackStockChangeHandler(MContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(TrackStockChangeRequest request, CancellationToken cancellationToken)
    {
        bool tracked = await _context.StockChanges
            .AsNoTracking()
            .AnyAsync(t =>
                t.MarketplaceId == request.MarketplaceId
                && t.WarehouseId == request.WarehouseId
                && t.ProductId == request.ProductId, cancellationToken);

        if (tracked)
        {
            return Unit.Value;
        }

        var change = _mapper.Map<StockChange>(request);
        await _context.StockChanges.AddAsync(change, cancellationToken);

        return Unit.Value;
    }
}
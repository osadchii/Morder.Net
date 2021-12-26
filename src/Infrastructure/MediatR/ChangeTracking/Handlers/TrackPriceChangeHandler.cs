using AutoMapper;
using Infrastructure.MediatR.ChangeTracking.Commands;
using Infrastructure.Models.Prices;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.ChangeTracking.Handlers;

public class TrackPriceChangeHandler : IRequestHandler<TrackPriceChangeRequest, Unit>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;

    public TrackPriceChangeHandler(MContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(TrackPriceChangeRequest request, CancellationToken cancellationToken)
    {
        bool tracked = await _context.PriceChanges
            .AsNoTracking()
            .AnyAsync(t =>
                t.MarketplaceId == request.MarketplaceId
                && t.ProductId == request.ProductId, cancellationToken);

        if (tracked)
        {
            return Unit.Value;
        }

        var change = _mapper.Map<PriceChange>(request);
        await _context.PriceChanges.AddAsync(change, cancellationToken);

        return Unit.Value;
    }
}
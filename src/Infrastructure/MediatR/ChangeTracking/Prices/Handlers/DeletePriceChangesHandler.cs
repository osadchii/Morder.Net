using Infrastructure.MediatR.ChangeTracking.Prices.Commands;
using Infrastructure.Models.Prices;
using MediatR;

namespace Infrastructure.MediatR.ChangeTracking.Prices.Handlers;

public class DeletePriceChangesHandler : IRequestHandler<DeletePriceChangesRequest, Unit>
{
    private readonly MContext _context;

    public DeletePriceChangesHandler(MContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeletePriceChangesRequest request, CancellationToken cancellationToken)
    {
        _context.RemoveRange(request.ProductIds.Select(p => new PriceChange
        {
            MarketplaceId = request.MarketplaceId,
            ProductId = p
        }));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
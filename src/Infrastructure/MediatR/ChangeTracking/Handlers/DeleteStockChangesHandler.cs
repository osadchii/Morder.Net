using Infrastructure.MediatR.ChangeTracking.Commands;
using Infrastructure.Models.Warehouses;
using MediatR;

namespace Infrastructure.MediatR.ChangeTracking.Handlers;

public class DeleteStockChangesHandler : IRequestHandler<DeleteStockChangesRequest, Unit>
{
    private readonly MContext _context;

    public DeleteStockChangesHandler(MContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteStockChangesRequest request, CancellationToken cancellationToken)
    {
        _context.RemoveRange(request.ProductIds.Select(p => new StockChange()
        {
            MarketplaceId = request.MarketplaceId,
            ProductId = p
        }));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
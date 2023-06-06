using Infrastructure.Common;
using Infrastructure.MediatR.Ozon.WarehouseBlacklists.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Ozon.WarehouseBlacklists.Commands;

public static class DeleteOzonWarehouseBlacklist
{
    public class Command : IRequest<Result>
    {
        public Command(int ozonId, int ozonWarehouseId, int productId)
        {
            OzonId = ozonId;
            OzonWarehouseId = ozonWarehouseId;
            ProductId = productId;
        }

        public int OzonId { get; }
        public int OzonWarehouseId { get; }
        public int ProductId { get; }
    }

    public class Handler : IRequestHandler<Command, Result>
    {
        private readonly MContext _context;

        public Handler(MContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var blacklist = await _context.OzonWarehouseBlacklists
                .Where(x => x.OzonWarehouseId == request.OzonWarehouseId)
                .Where(x => x.ProductId == request.ProductId)
                .FirstOrDefaultAsync(cancellationToken);

            if (blacklist is not null)
            {
                _context.OzonWarehouseBlacklists.Remove(blacklist);
                await _context.SaveChangesAsync(cancellationToken);
            }

            var blacklists = await _context.OzonWarehouseBlacklists
                .Where(x => x.OzonWarehouseId == request.OzonWarehouseId)
                .Include(x => x.Product)
                .ToListAsync(cancellationToken);

            var result = blacklists
                .Map()
                .AsResult();

            return result;
        }
    }
}
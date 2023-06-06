using Infrastructure.Common;
using Infrastructure.MediatR.Ozon.WarehouseBlacklists.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Ozon.WarehouseBlacklists.Queries;

public static class GetOzonWarehouseBlacklists
{
    public class Query : IRequest<Result>
    {
        public Query(int ozonId, int ozonWarehouseId)
        {
            OzonId = ozonId;
            OzonWarehouseId = ozonWarehouseId;
        }

        public int OzonId { get; }
        public int OzonWarehouseId { get; }
    }
    
    public class Handler : IRequestHandler<Query, Result>
    {
        private readonly MContext _context;

        public Handler(MContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            var blacklists = await _context.OzonWarehouseBlacklists
                .AsNoTracking()
                .Where(x => x.OzonWarehouseId == request.OzonWarehouseId)
                .Include(x => x.Product)
                .ToListAsync(cancellationToken);

            var result = blacklists.Map()
                .AsResult();

            return result;
        }
    }
}
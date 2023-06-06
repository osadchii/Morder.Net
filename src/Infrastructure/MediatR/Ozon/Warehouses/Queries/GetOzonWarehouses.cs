using Infrastructure.Common;
using Infrastructure.MediatR.Ozon.Warehouses.Mappings;
using Infrastructure.MediatR.Ozon.Warehouses.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Ozon.Warehouses.Queries;

public static class GetOzonWarehouses
{
    public class Query : IRequest<Result>
    {
        public Query(int ozonId)
        {
            OzonId = ozonId;
        }

        public int OzonId { get; }
    }
    
    public class Handler : IRequestHandler<Query, Result>
    {
        private readonly MContext _mContext;

        public Handler(MContext mContext)
        {
            _mContext = mContext;
        }

        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            var warehouses = await _mContext.OzonWarehouses
                .AsNoTracking()
                .Where(x => x.OzonId == request.OzonId)
                .ToListAsync(cancellationToken);

            var result = warehouses
                .Select(x => x.Map())
                .ToArray()
                .AsResult();

            return result;
        }
    }
}
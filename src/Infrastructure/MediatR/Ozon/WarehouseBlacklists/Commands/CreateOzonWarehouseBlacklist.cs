using Infrastructure.Common;
using Infrastructure.MediatR.Ozon.WarehouseBlacklists.Mappings;
using Infrastructure.MediatR.Ozon.WarehouseBlacklists.Models;
using Infrastructure.Models.Marketplaces.Ozon;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Ozon.WarehouseBlacklists.Commands;

public static class CreateOzonWarehouseBlacklist
{
    public class Command : IRequest<Result>
    {
        public Command(int ozonId, int ozonWarehouseId, OzonWarehouseBlacklistApplyModel model)
        {
            OzonId = ozonId;
            OzonWarehouseId = ozonWarehouseId;
            Model = model;
        }

        public int OzonId { get; }
        public int OzonWarehouseId { get; }
        public OzonWarehouseBlacklistApplyModel Model { get; }
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
            var model = request.Model;

            var exists = await _context.OzonWarehouseBlacklists
                .Where(x => x.OzonWarehouseId == request.OzonWarehouseId)
                .Where(x => x.ProductId == model.ProductId)
                .AnyAsync(cancellationToken);

            if (exists)
            {
                return new Result(ResultCode.Error)
                    .AddError("Product already exists in blacklist");
            }
            
            var entity = new OzonWarehouseBlacklist
            {
                OzonWarehouseId = request.OzonWarehouseId,
                ProductId = model.ProductId
            };
            
            await _context.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var blacklists = await _context.OzonWarehouseBlacklists
                .Where(x => x.OzonWarehouseId == request.OzonWarehouseId)
                .ToListAsync(cancellationToken);

            var result = blacklists
                .Map()
                .AsResult();

            return result;
        }
    }
}
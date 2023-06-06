using Infrastructure.Common;
using Infrastructure.MediatR.Ozon.Warehouses.Mappings;
using Infrastructure.MediatR.Ozon.Warehouses.Models;
using Infrastructure.Models.Marketplaces.Ozon;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Ozon.Warehouses.Commands;

public static class CreateOzonWarehouse
{
    public class Command : IRequest<Result>
    {
        public Command(int ozonId, OzonWarehouseApplyModel model)
        {
            OzonId = ozonId;
            Model = model;
        }

        public int OzonId { get; }
        public OzonWarehouseApplyModel Model { get; }
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

            var warehouseExists = await _context.OzonWarehouses
                .AsNoTracking()
                .Where(x => x.OzonId == request.OzonId)
                .Where(x => x.OzonWarehouseId == model.OzonWarehouseId)
                .AnyAsync(cancellationToken);

            if (warehouseExists)
            {
                return new Result(ResultCode.Error)
                    .AddError("Warehouse already exists");
            }

            var warehouseEntity = new OzonWarehouse
            {
                OzonId = request.OzonId,
                OzonWarehouseId = model.OzonWarehouseId,
                Name = model.Name
            };
            
            await _context.AddAsync(warehouseEntity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var result = warehouseEntity
                .Map()
                .AsResult();

            return result;
        }
    }
}
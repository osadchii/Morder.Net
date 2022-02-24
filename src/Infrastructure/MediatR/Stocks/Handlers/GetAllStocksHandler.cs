using Infrastructure.Cache.Interfaces;
using Infrastructure.Common;
using Infrastructure.MediatR.Stocks.Queries;
using Infrastructure.Models.Warehouses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.Stocks.Handlers;

public class GetAllStocksHandler : IRequestHandler<GetAllStocksRequest, Result>
{
    private readonly MContext _context;
    private readonly ILogger<GetAllStocksHandler> _logger;
    private readonly IIdExtractor<Warehouse> _warehouseIdExtractor;

    public GetAllStocksHandler(MContext context, ILogger<GetAllStocksHandler> logger,
        IIdExtractor<Warehouse> warehouseIdExtractor)
    {
        _context = context;
        _logger = logger;
        _warehouseIdExtractor = warehouseIdExtractor;
    }

    public async Task<Result> Handle(GetAllStocksRequest request, CancellationToken cancellationToken)
    {
        if (!request.WarehouseExternalId.HasValue)
        {
            return ResultCode.Error.AsResult("Bad request");
        }

        int? warehouseId = await _warehouseIdExtractor.GetIdAsync(request.WarehouseExternalId.Value);

        if (warehouseId is null)
        {
            _logger.LogWarning("Warehouse with external id {WarehouseExternalId} not found",
                request.WarehouseExternalId);
            return ResultCode.Error.AsResult($"Warehouse with external id {request.WarehouseExternalId} not found");
        }

        var stocks = await _context.Stocks
            .AsNoTracking()
            .Where(s => s.WarehouseId == warehouseId)
            .Include(s => s.Product)
            .Select(s => new { s.Product.ExternalId, s.Value })
            .ToListAsync(cancellationToken);

        return stocks.AsResult();
    }
}
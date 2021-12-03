using Infrastructure.Cache.Interfaces;
using Infrastructure.MediatR.Stocks.Commands;
using Infrastructure.Models.Products;
using Infrastructure.Models.Warehouses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.Stocks.Handlers;

public class UpdateStockHandler : IRequestHandler<UpdateStockRequest, Unit>
{
    private readonly MContext _context;
    private readonly ILogger<UpdateStockHandler> _logger;
    private readonly IIdExtractor<Product> _productIdExtractor;
    private readonly IIdExtractor<Warehouse> _warehouseIdExtractor;

    public UpdateStockHandler(MContext context, ILogger<UpdateStockHandler> logger,
        IIdExtractor<Product> productIdExtractor, IIdExtractor<Warehouse> warehouseIdExtractor)
    {
        _context = context;
        _logger = logger;
        _productIdExtractor = productIdExtractor;
        _warehouseIdExtractor = warehouseIdExtractor;
    }

    public async Task<Unit> Handle(UpdateStockRequest request, CancellationToken cancellationToken)
    {
        if (!request.WarehouseExternalId.HasValue || !request.ProductExternalId.HasValue)
        {
            return Unit.Value;
        }

        int? warehouseId = await _warehouseIdExtractor.GetIdAsync(request.WarehouseExternalId.Value);

        if (warehouseId is null)
        {
            _logger.LogWarning($"Warehouse with external id {request.WarehouseExternalId} not found");
            return Unit.Value;
        }

        int? productId = await _productIdExtractor.GetIdAsync(request.ProductExternalId.Value);

        if (productId is null)
        {
            _logger.LogWarning($"Product with external id {request.ProductExternalId} not found");
            return Unit.Value;
        }

        if (!request.Value.HasValue)
        {
            _logger.LogWarning($"Empty stock for product {productId.Value} at warehouse {warehouseId.Value}");
            return Unit.Value;
        }

        Stock? dbEntry = await _context.Stocks
            .SingleOrDefaultAsync(s => s.ProductId == productId && s.WarehouseId == warehouseId,
                cancellationToken);

        if (dbEntry is null)
        {
            return await CreateStock(warehouseId.Value,
                productId.Value, request.Value.Value, cancellationToken);
        }

        if (dbEntry.Value == request.Value.Value)
        {
            return Unit.Value;
        }

        return await UpdateStock(dbEntry, request.Value.Value, cancellationToken);
    }

    private async Task<Unit> CreateStock(int warehouseId, int productId, decimal value,
        CancellationToken cancellationToken)
    {
        var stock = new Stock
        {
            ProductId = productId,
            WarehouseId = warehouseId,
            Value = value
        };

        await _context.AddAsync(stock, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Created stock for product {productId} at warehouse {warehouseId} to {value}");

        return Unit.Value;
    }

    private async Task<Unit> UpdateStock(Stock dbEntry, decimal value,
        CancellationToken cancellationToken)
    {
        dbEntry.Value = value;

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            $"Updated stock for product {dbEntry.ProductId} at warehouse {dbEntry.WarehouseId} to {value}");

        return Unit.Value;
    }
}
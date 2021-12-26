using AutoMapper;
using Infrastructure.Cache.Interfaces;
using Infrastructure.MediatR.Marketplaces;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Prices;
using Infrastructure.Models.Warehouses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.Marketplaces;

public interface IMarketplaceUpdateService<in TRequest, TDto>
    where TRequest : BaseUpdateMarketplaceRequest, IRequest<TDto>
    where TDto : MarketplaceDto
{
    Task<TDto> UpdateMarketplaceAsync(TRequest request, CancellationToken cancellationToken);
}

public class MarketplaceUpdateService<TRequest, TDto> : IMarketplaceUpdateService<TRequest, TDto>
    where TRequest : BaseUpdateMarketplaceRequest, IRequest<TDto>
    where TDto : MarketplaceDto
{
    private readonly MContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<MarketplaceUpdateService<TRequest, TDto>> _logger;
    private readonly IIdExtractor<Warehouse> _warehouseIdExtractor;
    private readonly IIdExtractor<PriceType> _priceTypeIdExtractor;
    private readonly IChangeTrackingService _changeTrackingService;

    public MarketplaceUpdateService(MContext context,
        IMapper mapper,
        ILogger<MarketplaceUpdateService<TRequest, TDto>> logger,
        IIdExtractor<Warehouse> warehouseIdExtractor,
        IIdExtractor<PriceType> priceTypeIdExtractor,
        IChangeTrackingService changeTrackingService)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
        _warehouseIdExtractor = warehouseIdExtractor;
        _priceTypeIdExtractor = priceTypeIdExtractor;
        _changeTrackingService = changeTrackingService;
    }

    public async Task<TDto> UpdateMarketplaceAsync(TRequest request, CancellationToken cancellationToken)
    {
        int? warehouseId = await _warehouseIdExtractor.GetIdAsync(request.WarehouseExternalId!.Value);

        if (!warehouseId.HasValue)
        {
            throw new ArgumentException($"Wrong warehouse external id: {request.WarehouseExternalId}");
        }

        request.Warehouse = await _context.Warehouses
            .AsNoTracking()
            .SingleAsync(w => w.Id == warehouseId, cancellationToken);

        if (request.PriceTypeExternalId.HasValue)
        {
            int? priceTypeId = await _priceTypeIdExtractor.GetIdAsync(request.PriceTypeExternalId.Value);

            if (!priceTypeId.HasValue)
            {
                throw new ArgumentException($"Wrong price type external id: {request.WarehouseExternalId}");
            }

            request.PriceType = await _context.PriceTypes
                .AsNoTracking()
                .SingleAsync(w => w.Id == priceTypeId, cancellationToken);
        }

        _changeTrackingService.ResetCaches();

        Marketplace? marketplace;

        if (request.Id.HasValue)
        {
            marketplace = await _context.Marketplaces
                .Include(m => m.Warehouse)
                .Include(m => m.PriceType)
                .SingleAsync(m => m.Id == request.Id.Value, cancellationToken);
        }
        else
        {
            marketplace = null;
        }

        if (marketplace is null)
        {
            return await CreateMarketplace(request, cancellationToken);
        }

        return await UpdateMarketplace(marketplace, request, cancellationToken);
    }

    private async Task<TDto> CreateMarketplace(TRequest request, CancellationToken cancellationToken)
    {
        var dbEntry = _mapper.Map<Marketplace>(request);

        await _context.AddAsync(dbEntry, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        if (dbEntry.IsActive && dbEntry.PriceChangesTracking)
        {
            await _changeTrackingService.TrackAllPrices(dbEntry.Id, cancellationToken);
        }

        if (dbEntry.IsActive && dbEntry.StockChangesTracking)
        {
            await _changeTrackingService.TrackAllStocks(dbEntry.Id, cancellationToken);
        }

        _logger.LogInformation($@"Created marketplace {request.Name}");

        return _mapper.Map<TDto>(dbEntry);
    }

    private async Task<TDto> UpdateMarketplace(Marketplace dbEntry, TRequest request,
        CancellationToken cancellationToken)
    {
        decimal oldMinimalPrice = dbEntry.MinimalPrice;
        decimal oldMinimalStock = dbEntry.MinimalStock;

        bool oldIsActive = dbEntry.IsActive;
        bool oldTrackPrices = dbEntry.PriceChangesTracking;
        bool oldTrackStocks = dbEntry.StockChangesTracking;

        _mapper.Map(request, dbEntry);

        await _context.SaveChangesAsync(cancellationToken);

        if (request.PriceChangesTracking && request.IsActive)
        {
            if (!oldIsActive || !oldTrackPrices)
            {
                await _changeTrackingService.TrackAllPrices(dbEntry.Id, cancellationToken);
            }
        }

        if (request.StockChangesTracking && request.IsActive)
        {
            if (!oldIsActive || !oldTrackStocks || oldMinimalStock != request.MinimalStock)
            {
                await _changeTrackingService.TrackAllStocks(dbEntry.Id, cancellationToken);
            }
            else if (oldMinimalPrice != request.MinimalPrice)
            {
                await _changeTrackingService.TrackStockChangeByMinMaxPrice(dbEntry.Id,
                    Math.Min(oldMinimalPrice, request.MinimalPrice!.Value),
                    Math.Max(oldMinimalPrice, request.MinimalPrice!.Value), cancellationToken);
            }
        }

        _logger.LogInformation($"Updated marketplace {request.Name}");

        return _mapper.Map<TDto>(dbEntry);
    }
}
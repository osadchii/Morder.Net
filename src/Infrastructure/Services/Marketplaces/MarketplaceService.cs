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

    public MarketplaceUpdateService(MContext context,
        IMapper mapper,
        ILogger<MarketplaceUpdateService<TRequest, TDto>> logger,
        IIdExtractor<Warehouse> warehouseIdExtractor,
        IIdExtractor<PriceType> priceTypeIdExtractor)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
        _warehouseIdExtractor = warehouseIdExtractor;
        _priceTypeIdExtractor = priceTypeIdExtractor;
    }

    public async Task<TDto> UpdateMarketplaceAsync(TRequest request, CancellationToken cancellationToken)
    {
        int? warehouseId = await _warehouseIdExtractor.GetIdAsync(request.WarehouseExternalId!.Value);

        if (!warehouseId.HasValue)
        {
            throw new ArgumentException($"Wrong warehouse external id: {request.WarehouseExternalId}");
        }

        int? priceTypeId = await _priceTypeIdExtractor.GetIdAsync(request.PriceTypeExternalId!.Value);

        if (!priceTypeId.HasValue)
        {
            throw new ArgumentException($"Wrong price type external id: {request.WarehouseExternalId}");
        }

        request.Warehouse = await _context.Warehouses
            .AsNoTracking()
            .SingleAsync(w => w.Id == warehouseId, cancellationToken);

        request.PriceType = await _context.PriceTypes
            .AsNoTracking()
            .SingleAsync(w => w.Id == priceTypeId, cancellationToken);

        Marketplace? marketplace;

        if (request.Id.HasValue)
        {
            marketplace = await _context.Marketplaces
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

        _logger.LogInformation($"Created marketplace {request.Name}");

        return _mapper.Map<TDto>(dbEntry);
    }

    private async Task<TDto> UpdateMarketplace(Marketplace dbEntry, TRequest request,
        CancellationToken cancellationToken)
    {
        _mapper.Map(request, dbEntry);

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Updated marketplace {request.Name}");

        return _mapper.Map<TDto>(dbEntry);
    }
}
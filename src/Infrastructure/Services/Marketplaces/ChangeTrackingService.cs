using Infrastructure.Cache;
using Infrastructure.MediatR.ChangeTracking.Commands;
using Infrastructure.MediatR.Marketplaces.Common.Queries;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Services.Marketplaces;

public interface IChangeTrackingService
{
    Task<IEnumerable<int>> GetMarketplaceTrackingPriceIdsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<int>> GetMarketplaceTrackingStockIdsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<int>> GetTrackingPriceTypeIds(int marketplaceId, CancellationToken cancellationToken);
    Task<int> GetTrackingWarehouseId(int marketplaceId, CancellationToken cancellationToken);
    Task TrackStockChange(int marketplaceId, int warehouseId, int productId, CancellationToken cancellationToken);
    Task TrackPriceChange(int marketplaceId, int priceTypeId, int productId, CancellationToken cancellationToken);
    void ResetMarketplaceTrackingPriceIds();
    void ResetMarketplaceTrackingStockIds();
    void ResetTrackingPriceTypeIds();
    void ResetTrackingWarehouseIds();

    void ResetCaches()
    {
        ResetTrackingPriceTypeIds();
        ResetTrackingWarehouseIds();
        ResetMarketplaceTrackingPriceIds();
        ResetMarketplaceTrackingStockIds();
    }
}

public class ChangeTrackingService : IChangeTrackingService
{
    private readonly IMemoryCache _cache;
    private readonly IMediator _mediator;

    public ChangeTrackingService(IMemoryCache cache, IMediator mediator)
    {
        _cache = cache;
        _mediator = mediator;
    }

    public async Task<IEnumerable<int>> GetMarketplaceTrackingPriceIdsAsync(CancellationToken cancellationToken)
    {
        if (_cache.TryGetValue(CacheKeys.MarketplaceTrackingPriceIds, out List<int> ids))
        {
            return ids;
        }

        ids = await _mediator.Send(new GetMarketplaceTrackingPriceIdsRequest(), cancellationToken);

        _cache.Set(CacheKeys.MarketplaceTrackingPriceIds, ids);
        return ids;
    }

    public async Task<IEnumerable<int>> GetMarketplaceTrackingStockIdsAsync(CancellationToken cancellationToken)
    {
        if (_cache.TryGetValue(CacheKeys.MarketplaceTrackingStockIds, out List<int> ids))
        {
            return ids;
        }

        ids = await _mediator.Send(new GetMarketplaceTrackingStockIdsRequest(), cancellationToken);

        _cache.Set(CacheKeys.MarketplaceTrackingStockIds, ids);
        return ids;
    }

    public async Task<IEnumerable<int>> GetTrackingPriceTypeIds(int marketplaceId, CancellationToken cancellationToken)
    {
        List<int>? priceTypeIds;
        if (_cache.TryGetValue(CacheKeys.MarketplaceTrackingPriceTypeIds, out Dictionary<int, List<int>> ids))
        {
            if (ids.TryGetValue(marketplaceId, out priceTypeIds))
            {
                return priceTypeIds;
            }

            priceTypeIds = await _mediator.Send(new GetMarketplaceTrackingPriceTypeIdsRequest(marketplaceId),
                cancellationToken);
            ids.Add(marketplaceId, priceTypeIds);
            _cache.Set(CacheKeys.MarketplaceTrackingPriceTypeIds, ids);

            return priceTypeIds;
        }

        priceTypeIds = await _mediator.Send(new GetMarketplaceTrackingPriceTypeIdsRequest(marketplaceId),
            cancellationToken);
        var cachedDictionary = new Dictionary<int, List<int>>
        {
            [marketplaceId] = priceTypeIds
        };
        _cache.Set(CacheKeys.MarketplaceTrackingPriceTypeIds, cachedDictionary);

        return priceTypeIds;
    }

    public async Task<int> GetTrackingWarehouseId(int marketplaceId, CancellationToken cancellationToken)
    {
        int warehouseId;
        if (_cache.TryGetValue(CacheKeys.MarketplaceTrackingWarehouseIds, out Dictionary<int, int> ids))
        {
            if (ids.TryGetValue(marketplaceId, out warehouseId))
            {
                return warehouseId;
            }

            warehouseId = await _mediator.Send(new GetMarketplaceWarehouseIdRequest(marketplaceId), cancellationToken);
            ids.Add(marketplaceId, warehouseId);
            _cache.Set(CacheKeys.MarketplaceTrackingWarehouseIds, ids);

            return warehouseId;
        }

        warehouseId = await _mediator.Send(new GetMarketplaceWarehouseIdRequest(marketplaceId), cancellationToken);
        var cachedDictionary = new Dictionary<int, int>
        {
            [marketplaceId] = warehouseId
        };
        _cache.Set(CacheKeys.MarketplaceTrackingWarehouseIds, cachedDictionary);

        return warehouseId;
    }

    public Task TrackStockChange(int marketplaceId, int warehouseId, int productId, CancellationToken cancellationToken)
    {
        return _mediator.Send(new TrackStockChangeRequest(marketplaceId, warehouseId, productId), cancellationToken);
    }

    public Task TrackPriceChange(int marketplaceId, int priceTypeId, int productId, CancellationToken cancellationToken)
    {
        return _mediator.Send(new TrackPriceChangeRequest(marketplaceId, priceTypeId, productId), cancellationToken);
    }

    public void ResetMarketplaceTrackingPriceIds()
    {
        _cache.Remove(CacheKeys.MarketplaceTrackingPriceIds);
    }

    public void ResetMarketplaceTrackingStockIds()
    {
        _cache.Remove(CacheKeys.MarketplaceTrackingStockIds);
    }

    public void ResetTrackingPriceTypeIds()
    {
        _cache.Remove(CacheKeys.MarketplaceTrackingPriceTypeIds);
    }

    public void ResetTrackingWarehouseIds()
    {
        _cache.Remove(CacheKeys.MarketplaceTrackingWarehouseIds);
    }
}
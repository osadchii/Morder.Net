using Infrastructure.Cache;
using Infrastructure.MediatR.ChangeTracking.Prices.Commands;
using Infrastructure.MediatR.ChangeTracking.Stocks.Commands;
using Infrastructure.MediatR.Marketplaces.Common.Queries;
using Infrastructure.MediatR.Prices.Queries;
using Infrastructure.MediatR.Products.Queries;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Services.Marketplaces;

public interface IChangeTrackingService
{
    Task<IEnumerable<int>> GetMarketplaceTrackingPriceIdsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<int>> GetMarketplaceTrackingStockIdsAsync(CancellationToken cancellationToken);
    Task TrackStockChange(int marketplaceId, int productId, CancellationToken cancellationToken);
    Task TrackPriceChange(int marketplaceId, int productId, CancellationToken cancellationToken);
    Task TrackStocksChange(int marketplaceId, IEnumerable<int> productIds, CancellationToken cancellationToken);
    Task TrackPricesChange(int marketplaceId, IEnumerable<int> productIds, CancellationToken cancellationToken);
    Task TrackStockChange(int productId, CancellationToken cancellationToken);
    Task TrackPriceChange(int productId, CancellationToken cancellationToken);
    Task TrackAllPrices(int marketplaceId, CancellationToken cancellationToken);
    Task TrackAllStocks(int marketplaceId, CancellationToken cancellationToken);

    Task TrackStockChangeByMinMaxPrice(int marketplaceId, decimal minimalPrice, decimal maximalPrice,
        CancellationToken cancellationToken);

    void ResetMarketplaceTrackingPriceIds();
    void ResetMarketplaceTrackingStockIds();

    void ResetCaches()
    {
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

    public async Task TrackStockChange(int marketplaceId, int productId, CancellationToken cancellationToken)
    {
        var isProductTrackable = await _mediator.Send(new IsProductTrackableRequest
        {
            MarketplaceId = marketplaceId,
            ProductId = productId
        }, cancellationToken);

        if (!isProductTrackable)
        {
            return;
        }

        await _mediator.Send(new TrackStockChangeRequest(marketplaceId, productId), cancellationToken);
    }

    public async Task TrackPriceChange(int marketplaceId, int productId, CancellationToken cancellationToken)
    {
        var isProductTrackable = await _mediator.Send(new IsProductTrackableRequest
        {
            MarketplaceId = marketplaceId,
            ProductId = productId
        }, cancellationToken);

        if (!isProductTrackable)
        {
            return;
        }

        await _mediator.Send(new TrackPriceChangeRequest(marketplaceId, productId), cancellationToken);
    }

    public Task TrackStocksChange(int marketplaceId, IEnumerable<int> productIds, CancellationToken cancellationToken)
    {
        return _mediator.Send(new TrackStocksChangeRequest
        {
            MarketplaceId = marketplaceId,
            ProductIds = productIds
        }, cancellationToken);
    }

    public Task TrackPricesChange(int marketplaceId, IEnumerable<int> productIds, CancellationToken cancellationToken)
    {
        return _mediator.Send(new TrackPricesChangeRequest
        {
            MarketplaceId = marketplaceId,
            ProductIds = productIds
        }, cancellationToken);
    }

    public async Task TrackStockChange(int productId, CancellationToken cancellationToken)
    {
        var marketplaceIds =
            await GetMarketplaceTrackingStockIdsAsync(cancellationToken);

        foreach (var marketplaceId in marketplaceIds)
        {
            await TrackStockChange(marketplaceId, productId, cancellationToken);
        }
    }

    public async Task TrackPriceChange(int productId, CancellationToken cancellationToken)
    {
        var marketplaceIds =
            await GetMarketplaceTrackingPriceIdsAsync(cancellationToken);

        foreach (var marketplaceId in marketplaceIds)
        {
            await TrackPriceChange(marketplaceId, productId, cancellationToken);
        }
    }

    public Task TrackAllPrices(int marketplaceId, CancellationToken cancellationToken)
    {
        return _mediator.Send(new TrackAllPricesRequest { MarketplaceId = marketplaceId }, cancellationToken);
    }

    public Task TrackAllStocks(int marketplaceId, CancellationToken cancellationToken)
    {
        return _mediator.Send(new TrackAllStocksRequest { MarketplaceId = marketplaceId }, cancellationToken);
    }

    public async Task TrackStockChangeByMinMaxPrice(int marketplaceId, decimal minimalPrice, decimal maximalPrice,
        CancellationToken token)
    {
        var productIds = await _mediator.Send(new GetProductIdsInMarketplacePriceRangeRequest
        {
            MarketplaceId = marketplaceId,
            MinimalPrice = minimalPrice,
            MaximalPrice = maximalPrice
        }, token);

        await TrackStocksChange(marketplaceId, productIds, token);
    }

    public void ResetMarketplaceTrackingPriceIds()
    {
        _cache.Remove(CacheKeys.MarketplaceTrackingPriceIds);
    }

    public void ResetMarketplaceTrackingStockIds()
    {
        _cache.Remove(CacheKeys.MarketplaceTrackingStockIds);
    }
}
using System.Net;
using AutoMapper;
using Infrastructure;
using Infrastructure.Cache.Interfaces;
using Infrastructure.Marketplaces;
using Infrastructure.MediatR.Orders.Commands;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Infrastructure.Models.Orders;
using Integration.SberMegaMarket.Orders.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Integration.SberMegaMarket.Orders;

public interface ISberMegaMarketOrderAdapter
{
    public Task<IEnumerable<CreateOrderRequest>> CreateOrderRequests(
        BaseSberMegaMarketOrderRequest<CreateSberMegaMarketOrdersRequest> createRequest);

    public Task<IEnumerable<CancelOrderItemsByExternalIdRequest>> CancelOrderRequests(
        BaseSberMegaMarketOrderRequest<CancelSberMegaMarketOrdersRequest> cancelRequest);
}

public class SberMegaMarketOrderAdapter : ISberMegaMarketOrderAdapter
{
    private readonly IMemoryCache _cache;
    private readonly MContext _context;
    private readonly IMapper _mapper;
    private readonly IProductCache _productCache;

    public SberMegaMarketOrderAdapter(IMemoryCache cache, MContext context, IMapper mapper, IProductCache productCache)
    {
        _cache = cache;
        _context = context;
        _mapper = mapper;
        _productCache = productCache;
    }

    public async Task<IEnumerable<CreateOrderRequest>> CreateOrderRequests(
        BaseSberMegaMarketOrderRequest<CreateSberMegaMarketOrdersRequest> createRequest)
    {
        CreateSberMegaMarketOrdersRequest data = createRequest.Data;
        SberMegaMarketDto sber = await GetMarketplaceByMerchantId(data.MerchantId);
        List<string> requestArticuls = data.Shipments
            .SelectMany(s => s.Items.Select(i => i.OfferId))
            .Distinct()
            .ToList();

        Dictionary<string, int> products = await _productCache.GetProductIdsByArticul(requestArticuls);

        return data.Shipments.Select(s => new CreateOrderRequest()
        {
            Customer = "SberMegaMarket Customer",
            Date = DateTime.SpecifyKind(s.ShipmentDate, DateTimeKind.Utc),
            Number = s.ShipmentId,
            Status = OrderStatus.Created,
            ExternalId = Guid.NewGuid(),
            MarketplaceId = sber.Id,
            ShippingDate = DateTime.SpecifyKind(s.Shipping.ShippingDate, DateTimeKind.Utc),
            Items = s.Items.Select(i => new CreateOrderItem()
            {
                Count = i.Quantity,
                Price = i.Price,
                Sum = i.Quantity * i.Price,
                ExternalId = i.ItemIndex,
                ProductId = products[i.OfferId]
            })
        });
    }

    public async Task<IEnumerable<CancelOrderItemsByExternalIdRequest>> CancelOrderRequests(
        BaseSberMegaMarketOrderRequest<CancelSberMegaMarketOrdersRequest> cancelRequest)
    {
        CancelSberMegaMarketOrdersRequest data = cancelRequest.Data;
        SberMegaMarketDto sber = await GetMarketplaceByMerchantId(data.MerchantId);

        return data.Shipments.Select(s => new CancelOrderItemsByExternalIdRequest()
        {
            MarketplaceId = sber.Id,
            OrderNumber = s.ShipmentId,
            ItemExternalIds = s.Items.Select(i => i.ItemIndex)
        });
    }

    private async Task<SberMegaMarketDto> GetMarketplaceByMerchantId(int merchantId)
    {
        const string cacheKeyBase = "SberMegaMarketByMerchantId";
        string cacheKey = $"{cacheKeyBase}_{merchantId}";

        if (_cache.TryGetValue(cacheKey, out SberMegaMarketDto sber))
        {
            return sber;
        }

        IEnumerable<Marketplace> marketplace = await _context.Marketplaces
            .AsNoTracking()
            .Where(m => m.IsActive && m.Type == MarketplaceType.SberMegaMarket)
            .ToListAsync();

        SberMegaMarketDto? sberContext = marketplace
            .Select(m => _mapper.Map<SberMegaMarketDto>(m))
            .SingleOrDefault(s => s.Settings.MerchantId == merchantId);

        if (sberContext is null)
        {
            throw new HttpRequestException($"Marketplace with {merchantId} merchant id not found", null,
                HttpStatusCode.NotFound);
        }

        _cache.Set(cacheKey, sberContext);

        return sberContext;
    }
}
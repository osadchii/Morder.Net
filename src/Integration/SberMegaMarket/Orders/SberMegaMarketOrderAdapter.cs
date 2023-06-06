using System.Net;
using AutoMapper;
using Infrastructure;
using Infrastructure.Cache.Interfaces;
using Infrastructure.Extensions;
using Infrastructure.MediatR.Orders.Marketplace.Common.Commands;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Infrastructure.Models.Orders;
using Integration.SberMegaMarket.Clients.Orders;
using Integration.SberMegaMarket.Clients.Orders.Messages;
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

    public Task<CreateOrdersRequest>
        CreateOrdersRequest(UpdateOrderResponseDataShipment[] shipments, SberMegaMarketDto sber);
}

public class SberMegaMarketOrderAdapter : ISberMegaMarketOrderAdapter
{
    private readonly IMemoryCache _cache;
    private readonly MContext _context;
    private readonly IMapper _mapper;
    private readonly IProductCache _productCache;
    private const string Customer = "SberMegaMarket Customer";
    private readonly DateTime _wrongStatusDate = new DateTime(2021, 1, 1);

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
        var requestArticuls = data.Shipments
            .SelectMany(s => s.Items.Select(i => i.OfferId))
            .Distinct()
            .ToList();

        var products = await _productCache.GetProductIdsByArticul(requestArticuls);

        return data.Shipments.Select(s => new CreateOrderRequest()
        {
            Customer = Customer,
            Date = s.ShipmentDate.ToCommonTime().ToUtcTime(),
            Number = s.ShipmentId,
            Status = OrderStatus.Created,
            ExternalId = Guid.NewGuid(),
            MarketplaceId = sber.Id,
            ShippingDate = s.Shipping.ShippingDate.ToCommonTime().ToUtcTime(),
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

    public async Task<CreateOrdersRequest> CreateOrdersRequest(UpdateOrderResponseDataShipment[] shipments,
        SberMegaMarketDto sber)
    {
        var result = new CreateOrdersRequest();
        var requestArticuls = shipments
            .SelectMany(s => s.Items.Select(i => i.OfferId))
            .Distinct()
            .ToList();

        var products = await _productCache.GetProductIdsByArticul(requestArticuls, true);

        result.CreateOrderRequests = shipments
            .Where(s => s.Items.All(i => products.ContainsKey(i.OfferId)))
            .Select(s =>
            {
                DateTime creationDate = s.CreationDate.ToCommonTime().ToUtcTime();
                return new CreateOrderRequest()
                {
                    Archived = sber.Settings.LoadOrdersAsArchived,
                    Customer = s.CustomerFullName,
                    Number = s.ShipmentId,
                    Date = s.CreationDate.ToCommonTime().ToUtcTime(),
                    ExpressOrder = false,
                    ExternalId = Guid.NewGuid(),
                    MarketplaceId = sber.Id,
                    ShippingDate = s.ShippingDate.HasValue
                        ? s.ShippingDate.Value.ToCommonTime().ToUtcTime()
                        : new DateTime().ToUtcTime(),
                    Status = StatusConverter.GetOrderStatusBySberMegaMarketOrder(s, creationDate < _wrongStatusDate),
                    ConfirmedTimeLimit = s.ConfirmedTimeLimit.HasValue
                        ? s.ConfirmedTimeLimit.Value.ToCommonTime().ToUtcTime()
                        : new DateTime().ToUtcTime(),
                    PackingTimeLimit = s.PackingTimeLimit.HasValue
                        ? s.PackingTimeLimit.Value.ToCommonTime().ToUtcTime()
                        : new DateTime().ToUtcTime(),
                    ShippingTimeLimit = s.ShippingTimeLimit.HasValue
                        ? s.ShippingTimeLimit.Value.ToCommonTime().ToUtcTime()
                        : new DateTime().ToUtcTime(),
                    Items = s.Items.Select(i => new CreateOrderItem()
                    {
                        Count = i.Quantity,
                        Price = i.Price,
                        Sum = i.Quantity * i.Price,
                        ExternalId = i.ItemIndex,
                        ProductId = products[i.OfferId]
                    })
                };
            });

        return result;
    }

    private async Task<SberMegaMarketDto> GetMarketplaceByMerchantId(int merchantId)
    {
        const string cacheKeyBase = "SberMegaMarketByMerchantId";
        var cacheKey = $"{cacheKeyBase}_{merchantId}";

        if (_cache.TryGetValue(cacheKey, out SberMegaMarketDto sber))
        {
            return sber;
        }

        IEnumerable<Marketplace> marketplace = await _context.Marketplaces
            .AsNoTracking()
            .Where(m => m.IsActive && m.Type == MarketplaceType.SberMegaMarket)
            .ToListAsync();

        SberMegaMarketDto sberContext = marketplace
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
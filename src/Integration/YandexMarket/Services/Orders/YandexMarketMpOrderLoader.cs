using AutoMapper;
using Infrastructure;
using Infrastructure.Extensions;
using Infrastructure.MediatR.Orders.Marketplace.Common.Commands;
using Infrastructure.MediatR.Orders.Marketplace.Common.Queries;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.YandexMarket;
using Infrastructure.Models.Orders;
using Integration.Common.Services.Orders;
using Integration.YandexMarket.Clients;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.YandexMarket.Services.Orders;

public class YandexMarketMpOrderLoader : MarketplaceOrderLoader
{
    private readonly YandexMarketDto _marketDto;

    public YandexMarketMpOrderLoader(Marketplace marketplace, IServiceProvider serviceProvider, IMapper mapper,
        DateTime startDate) : base(marketplace, serviceProvider, mapper, startDate)
    {
        _marketDto = Mapper.Map<YandexMarketDto>(marketplace);
    }

    public override async Task LoadOrders()
    {
        if (!_marketDto.Settings.LoadArchiveOrders)
        {
            return;
        }

        var client = ServiceProvider.GetRequiredService<IYandexMarketMpGetOrdersClient>();
        var orders = await client.GetOrders(_marketDto);

        var mediatr = ServiceProvider.GetRequiredService<IMediator>();

        var doesNotExists = (await mediatr.Send(new OrdersDoesNotExistsRequest()
        {
            MarketplaceId = _marketDto.Id,
            Numbers = orders.Where(o => !o.Fake && o.Items.Any(i => !i.Canceled)).Select(r => r.Number)
        })).ToArray();

        if (!doesNotExists.Any())
        {
            return;
        }

        var context = ServiceProvider.GetRequiredService<MContext>();
        var productGuids = orders
            .SelectMany(o => o.Items.Select(i => i.OfferGuid))
            .Distinct();

        var products = await context.Products
            .AsNoTracking()
            .Where(p => productGuids.Contains(p.ExternalId))
            .ToDictionaryAsync(p => p.ExternalId, p => p.Id);

        var toCreate = orders.IntersectBy(doesNotExists, o => o.Number);

        var request = new CreateOrdersRequest
        {
            CreateOrderRequests = toCreate
                .Select(o =>
                {
                    return new CreateOrderRequest()
                    {
                        Archived = true,
                        Customer = "Yandex.Market Customer",
                        Date = o.Date.ToCommonTime().ToUtcTime(),
                        Number = o.Number,
                        ExternalId = Guid.NewGuid(),
                        MarketplaceId = _marketDto.Id,
                        ShippingDate = o.ShippingDate.ToCommonTime().ToUtcTime(),
                        Items = o.Items.Where(i => !i.Canceled).Select(i => new CreateOrderItem()
                        {
                            Count = i.Count,
                            Price = i.Price,
                            ProductId = products[i.OfferGuid],
                            Sum = i.Count * i.Price
                        }),
                        Status = o.Status switch
                        {
                            "CANCELLED_MERCHANT" => OrderStatus.Canceled,
                            _ => OrderStatus.Finished
                        }
                    };
                })
        };

        await mediatr.Send(request);
    }
}
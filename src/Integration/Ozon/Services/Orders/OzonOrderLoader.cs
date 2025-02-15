using AutoMapper;
using Infrastructure.MediatR.Orders.Marketplace.Common.Commands;
using Infrastructure.MediatR.Orders.Marketplace.Common.Queries;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Ozon;
using Integration.Common.Services.Orders;
using Integration.Ozon.Clients.Orders;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Integration.Ozon.Services.Orders;

public class OzonOrderLoader : MarketplaceOrderLoader
{
    private readonly OzonDto _ozon;

    public OzonOrderLoader(Marketplace marketplace, IServiceProvider serviceProvider, IMapper mapper,
        DateTime startDate)
        : base(marketplace, serviceProvider, mapper, startDate)
    {
        _ozon = Mapper.Map<OzonDto>(Marketplace);
    }

    public override async Task LoadOrders()
    {
        if (!_ozon.Settings.LoadOrders)
        {
            return;
        }

        var logger = ServiceProvider.GetRequiredService<ILogger<OzonOrderLoader>>();
        var client = ServiceProvider.GetRequiredService<IOzonLoadOrderListClient>();

        var postings = await client.GetOrders(_ozon, StartDate);

        logger.LogInformation("Loaded {Count} orders from Ozon", postings.Count);

        if (postings.Count == 0)
        {
            return;
        }

        var adapter = ServiceProvider.GetRequiredService<IOzonOrderAdapter>();
        var mediatr = ServiceProvider.GetRequiredService<IMediator>();

        var doesNotExists = (await mediatr.Send(new OrdersDoNotExistRequest
        {
            MarketplaceId = _ozon.Id,
            Numbers = postings.Select(r => r.PostingNumber)
        })).ToArray();

        logger.LogInformation("Need to create {Count} orders from Ozon", doesNotExists.Length);

        var requests = await adapter
            .CreateOrderRequests(_ozon, postings.IntersectBy(doesNotExists, x => x.PostingNumber).ToArray());

        await mediatr.Send(new CreateOrdersRequest
        {
            CreateOrderRequests = requests
        });
    }
}
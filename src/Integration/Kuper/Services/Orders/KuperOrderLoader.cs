using AutoMapper;
using Infrastructure.Extensions;
using Infrastructure.MediatR.Orders.Marketplace.Common.Commands;
using Infrastructure.MediatR.Orders.Marketplace.Common.Queries;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Kuper;
using Integration.Common.Services.Orders;
using Integration.Kuper.Clients.Orders;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Integration.Kuper.Services.Orders;

public class KuperOrderLoader : MarketplaceOrderLoader
{
    private readonly KuperDto _kuper;
    public KuperOrderLoader(Marketplace marketplace, IServiceProvider serviceProvider, IMapper mapper, DateTime startDate) : base(marketplace, serviceProvider, mapper, startDate)
    {
        _kuper = Mapper.Map<KuperDto>(Marketplace);
    }

    public override async Task LoadOrders()
    {
        if (_kuper.Settings.LoadOrders != true)
        {
            return;
        }

        var logger = ServiceProvider.GetRequiredService<ILogger<KuperOrderLoader>>();
        var client = ServiceProvider.GetRequiredService<IKuperOrdersClient>();

        var postings = await client.GetOrders(_kuper);

        logger.LogInformation("Loaded {Count} orders from Kuper", postings.Count);

        if (postings.Count == 0)
        {
            return;
        }

        var adapter = ServiceProvider.GetRequiredService<IKuperOrderAdapter>();
        var mediatr = ServiceProvider.GetRequiredService<IMediator>();

        var toCreate = (await mediatr.Send(new OrdersDoNotExistRequest
        {
            MarketplaceId = _kuper.Id,
            Numbers = postings.Select(r => r.OriginalOrderId)
        })).ToArray();

        logger.LogInformation("Need to create {Count} orders from Kuper", toCreate.Length);

        var requests = await adapter
            .CreateOrderRequests(_kuper, postings.IntersectBy(toCreate, x => x.OriginalOrderId).ToArray());

        await mediatr.Send(new CreateOrdersRequest
        {
            CreateOrderRequests = requests
        });
    }
}
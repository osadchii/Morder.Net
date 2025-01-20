using AutoMapper;
using Infrastructure.MediatR.Marketplaces.Common.Queries;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Meso;
using Integration.Common.Services.Feeds;
using Integration.Meso.Clients;
using Integration.Meso.Extensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Meso.Feeds;

public class MesoFeedService : MarketplaceFeedService
{
    private readonly MesoDto _meso;

    public MesoFeedService(IMapper mapper, IServiceProvider serviceProvider, Marketplace marketplace) : base(mapper,
        serviceProvider, marketplace)
    {
        _meso = mapper.Map<MesoDto>(Marketplace);
    }

    public override async Task GenerateFeed()
    {
        var mediator = ServiceProvider.GetRequiredService<IMediator>();
        var client = ServiceProvider.GetRequiredService<IMesoSendFeedClient>();

        var data = await mediator.Send(new GetMarketplaceProductDataRequest
        {
            MarketplaceId = _meso.Id
        });

        var feedBuilder = new FeedBuilder(data, _meso);

        var feed = feedBuilder.Build();
        await client.SendFeed(_meso, feed);

        if (_meso.Settings.SaveFeed)
        {
            var feedPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "feeds");
            var path = Path.Combine(feedPath, $"{_meso.Settings.FeedName}.json");
            if (!Directory.Exists(feedPath))
            {
                Directory.CreateDirectory(feedPath);
            }

            await feed.Save(path);
        }
    }
}
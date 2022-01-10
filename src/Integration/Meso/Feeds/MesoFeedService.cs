using AutoMapper;
using Infrastructure.MediatR.Companies.Queries;
using Infrastructure.MediatR.Marketplaces.Common.Queries;
using Infrastructure.Models.Companies;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Meso;
using Integration.Common.Services.Feeds;
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

        CompanyDto companyInformation = await mediator.Send(new GetCompanyInformationRequest());

        MarketplaceProductData data = await mediator.Send(new GetMarketplaceProductDataRequest()
        {
            MarketplaceId = _meso.Id
        });

        var feedBuilder = new FeedBuilder(data, _meso);

        Feed feed = feedBuilder.Build();

        if (_meso.Settings.SaveFeed)
        {
            string feedPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "feeds");
            string path = Path.Combine(feedPath, $"{_meso.Settings.FeedName}.json");
            if (!Directory.Exists(feedPath))
            {
                Directory.CreateDirectory(feedPath);
            }

            await feed.Save(path);
        }
    }
}
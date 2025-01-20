using AutoMapper;
using Infrastructure.MediatR.Companies.Queries;
using Infrastructure.MediatR.Marketplaces.Common.Queries;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Integration.Common.Services.Feeds;
using Integration.SberMegaMarket.Extensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.SberMegaMarket.Feeds;

public class SberMegaMarketFeedService : MarketplaceFeedService
{
    private readonly SberMegaMarketDto _sber;

    public SberMegaMarketFeedService(IMapper mapper, IServiceProvider serviceProvider, Marketplace marketplace) : base(
        mapper, serviceProvider, marketplace)
    {
        _sber = mapper.Map<SberMegaMarketDto>(marketplace);
    }

    public override async Task GenerateFeed()
    {
        if (!_sber.Settings.FeedEnabled)
        {
            return;
        }

        var mediator = ServiceProvider.GetRequiredService<IMediator>();

        var companyInformation = await mediator.Send(new GetCompanyInformationRequest());

        var data = await mediator.Send(new GetMarketplaceProductDataRequest()
        {
            MarketplaceId = _sber.Id
        });

        var feedBuilder = new FeedBuilder(data, _sber)
            .AddCompanyInformation(companyInformation);

        var feed = feedBuilder.Build();

        var feedPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "feeds");
        var path = Path.Combine(feedPath, $"{_sber.Settings.FeedName}.xml");
        if (!Directory.Exists(feedPath))
        {
            Directory.CreateDirectory(feedPath);
        }

        feed.Save(path);
    }
}